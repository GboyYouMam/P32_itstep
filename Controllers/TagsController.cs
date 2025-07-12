using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Entities;

namespace WebApplication1.Controllers;

public class TagsController : Controller
{
    private readonly SqLiteDbContext _context;

    public TagsController(SqLiteDbContext context)
    {
        _context = context;
    }

    // GET: Tags
    public async Task<IActionResult> Index()
    {
        var tags = await _context.Tags.Include(t => t.Notes).ToListAsync();
        var viewModel = tags.Select(t => Mappers.TagMapper.MapToViewModel(t)).ToList();
        return View(viewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        var viewModel = Mappers.TagMapper.MapToViewModel(tag);
        var notes = tag.Notes.ToList();
        ViewBag.Notes = notes; // додано для передачі нотаток у вʼюшку
        return View(viewModel);
    }
    
    public async Task<TagEntity> GetTagByIdAsync(int id)
    {
        return await _context.Tags.Include(t => t.Notes).FirstOrDefaultAsync(t => t.Id == id);
    }
}