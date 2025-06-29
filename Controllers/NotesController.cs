using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
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
}