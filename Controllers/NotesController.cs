using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Entities;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class NotesController : Controller
{
    private readonly SqLiteDbContext _context;
    private readonly ILogger<NotesController> _logger;

    public NotesController(SqLiteDbContext context, ILogger<NotesController> logger)
    {
        _logger = logger;
        _context = context;
    }
    
    // GET: Notes
    [HttpGet]
    public IActionResult Index()
    {
        return View(_context.Notes.Include(n => n.Tags).ToList());
    }
    
    // GET: Notes/Create
    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name");
        return View();
    }
    
    // POST: Notes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NoteViewModel model)
    {
        if (ModelState.IsValid)
        {
            var tags = _context.Tags.Where(t => model.TagsId.Contains(t.Id)).ToList();
            var note = NoteMapper.MapToEntity(model, tags);
            _context.Notes.Add(note);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name", model.TagsId);
        return View(model);
    }
    
    private async Task<bool> NoteExists(int? id)
    {
        return await _context.Notes.AnyAsync(e => e.Id == id);
    }
    
    public async Task<NoteEntity?> FindNoteById(int? id)
    {
        if (!await NoteExists(id))
        {
            _logger.LogWarning("Note with ID {Id} does not exist.", id);
            return null;
        }
        
        var note = await _context.Notes
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Id == id);
        if (note == null)
        {
            return null;
        }
        return note;
    }
    
    //GET: Notes/Edit/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!await NoteExists(id))
        {
            _logger.LogWarning("Note with ID {Id} does not exist.", id);
            return NotFound();
        }
        var note = await FindNoteById(id);
        
        return View(note);
    }
    
    // GET: Notes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var allTags = await _context.Tags.ToListAsync();
        
        var note = await FindNoteById(id);
        
        if (note == null)
        {
            return NotFound();
        }
        
        ViewBag.Tags = new MultiSelectList(allTags, "Id", "Name", note?.Tags.Select(t => t.Id).ToList());
        
        return View(Mappers.NoteMapper.MapToViewModel(note));
    }
    
    // POST: Notes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,TagsId")] NoteViewModel noteVM)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var note = await FindNoteById(id);
                if (note == null)
                {
                    return NotFound();
                }
                
                note = NoteMapper.MapToEntity(
                    note, 
                    noteVM, 
                    await _context.Tags
                        .Where(t => noteVM.TagsId.Contains(t.Id))
                        .ToListAsync());
                
                _context.Update(note);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await NoteExists(id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name", noteVM.TagsId);
        return View(noteVM);
    }
    
    // GET: Notes/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        var note = await FindNoteById(id);
        if (note == null)
        {
            return NotFound();
        }
        
        return View(note);
    }
    
    // POST: Notes/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var note = await FindNoteById(id);
        if (note == null)
        {
            return NotFound();
        }
        
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}