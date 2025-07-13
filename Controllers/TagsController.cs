using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Entities;
using WebApplication1.Models;

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
    
    //GET: Tags/Delete/1
    public async Task<IActionResult> Delete(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        var notes = tag.Notes.ToList();
        ViewBag.Notes = notes; // додано для передачі нотаток у вʼюшку
        var viewModel = Mappers.TagMapper.MapToViewModel(tag);
        return View(viewModel);
    }
    
    // POST: Tags/Delete/1
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    
    // GET: Tags/Create
    public IActionResult Create()
    {
        return View();
    }
    // POST: Tags/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name")] TagViewModel tagVM)
    {
        if (ModelState.IsValid)
        {
            var tagEntity = new TagEntity
            {
                Name = tagVM.Name
            };
            _context.Tags.Add(tagEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tagVM);
    }
    
    // GET: Tags/Edit/1
    public async Task<IActionResult> Edit(int id)
    {
        var tag = await GetTagByIdAsync(id);
        if (tag == null)
        {
            return NotFound();
        }
        var viewModel = Mappers.TagMapper.MapToViewModel(tag);
        return View(viewModel);
    }
    // POST: Tags/Edit/1
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] TagViewModel tagVM)
    {
        if (id != tagVM.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var tagEntity = await GetTagByIdAsync(id);
                if (tagEntity == null)
                {
                    return NotFound();
                }
                tagEntity.Name = tagVM.Name;
                _context.Update(tagEntity);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await TagExistsAsync(id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(tagVM);
    }
    
    public async Task<bool> TagExistsAsync(int id)
    {
        return await _context.Tags.AnyAsync(e => e.Id == id);
    }
}