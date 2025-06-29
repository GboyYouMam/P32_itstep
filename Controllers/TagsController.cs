using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;

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
        var tags = await _context.Tags.ToListAsync();
        return View(tags);
    }

    // GET: Tags/Create
    public IActionResult Create()
    {
        return View();
    }
}