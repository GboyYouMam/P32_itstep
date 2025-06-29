using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;

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
    public async Task<IActionResult> Index()
    {
        return View(_context.Notes.Include(n => n.Tags).OrderByDescending(n => n.CreatedAt).ToList());
    }
}