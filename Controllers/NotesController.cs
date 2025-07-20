using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Entities;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

[Authorize]
public class NotesController : Controller
{
    private readonly SqLiteDbContext _context;
    private readonly ILogger<NotesController> _logger;
    private readonly UserManager<MyIdentityUserModel> _userManager;

    public NotesController(SqLiteDbContext context, ILogger<NotesController> logger, UserManager<MyIdentityUserModel> userManager)
    {
        _userManager = userManager;
        _logger = logger;
        _context = context;
    }
    
    // GET: Notes
    [HttpGet]
    public IActionResult Index()
    {
        var userId = _userManager.GetUserId(User);
        var notes = _context.Notes
            .Where(n => n.UserId == userId)
            .ToList();
        var viewModels = notes.Select(n => NoteMapper.MapToViewModel(n)).ToList();
        return View(viewModels);
    }
    
    public async Task<List<TagEntity>> GetAllTagsAsync()
    {
        return await _context.Tags.ToListAsync();
    }
    
    public List<TagEntity> GetAllTags()
    {
        return _context.Tags.ToList();
    }
    
    public List<TagEntity> GetUserTags()
    {
        var userId = _userManager.GetUserId(User);
        return _context.Tags.Where(t => t.UserId == userId).ToList();
    }

    public List<TagEntity> GetTagsFromNote(NoteViewModel note)
    {
        var userTags = GetUserTags();
        return userTags.Where(t => note.TagsId.Contains(t.Id)).ToList();
    }
    
    // GET: Notes/Create
    [HttpGet]
    public IActionResult Create()
    {
        var userId = _userManager.GetUserId(User);
        var userTags = GetUserTags();
        
        ViewBag.Tags = ViewBag.Tags = new MultiSelectList(userTags, "Id", "Name");
        return View();
    }
    
    // POST: Notes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(NoteViewModel model)
    {
        if (ModelState.IsValid)
        {
            var tags = GetUserTags().Where(t => model.TagsId.Contains(t.Id)).ToList();
            var userId = _userManager.GetUserId(User);
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            var note = NoteMapper.MapToEntity(model, tags, userId);
            _context.Notes.Add(note);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Tags = new SelectList(_context.Tags, "Id", "Name", model.TagsId);
        return View(model);
    }
    
    private async Task<bool> NoteExists(int? id)
    {
        return await _context.Users.Include(u => u.Notes)
            .AnyAsync(u => u.Id == _userManager.GetUserId(User) && u.Notes.Any(n => n.Id == id));
    }
    
    public async Task<NoteEntity?> FindNoteById(int? id)
    {
        if (!await NoteExists(id))
        {
            _logger.LogWarning("Note with ID {Id} does not exist.", id);
            return null;
        }
        
        var note = await _context.Users.Include(u => u.Notes)
            .ThenInclude(n => n.Tags)
            .SelectMany(u => u.Notes)
            .FirstOrDefaultAsync(n => n.Id == id);
        if (note == null)
        {
            return null;
        }
        return note;
    }
    
    // GET: Notes/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (!await NoteExists(id))
        {
            _logger.LogWarning("Note with ID {Id} does not exist.", id);
            return NotFound();
        }
        
        var note = await FindNoteById(id);
        ViewBag.Tags = note?.Tags.ToList();
        
        return View(Mappers.NoteMapper.MapToViewModel(note));
    }
    
    // GET: Notes/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var userTags =  GetUserTags();
        
        var note = await FindNoteById(id);
        
        if (note == null)
        {
            return NotFound();
        }
        
        ViewBag.Tags = new MultiSelectList(userTags, "Id", "Name", note?.Tags.Select(t => t.Id).ToList());
        
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
                
                var userId = _userManager.GetUserId(User);
                var userTags = GetUserTags();
                
                note = NoteMapper.MapToEntity(
                    note, 
                    noteVM, 
                    userTags
                        .Where(t => noteVM.TagsId.Contains(t.Id))
                        .ToList());
                
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

        ViewBag.Tags = note?.Tags.ToList();
        
        return View(Mappers.NoteMapper.MapToViewModel(note));
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
        var user = _context.Users.FirstOrDefault(u => u.Id == _userManager.GetUserId(User));
        user.Notes.Remove(note);
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}