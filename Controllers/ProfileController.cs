/*using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Db;
using WebApplication1.Models;

namespace WebApplication1.Controllers;

public class ProfileController : Controller
{
    private readonly SqLiteDbContext _context;

    public ProfileController(SqLiteDbContext context)
    {
        _context = context;
    }
    
    // GET: Profile
    public async Task<IActionResult> Index()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();

        var user = await _context.ProfileUsers
            .Include(p => p.Tags)
            .Include(p => p.Notes)
            .FirstOrDefaultAsync(p => p.MyIdentityUserId == userId);

        if (user == null) return NotFound();

        var model = Mappers.ProfileMapper.MapToViewModel(user);
        return View(model);
    }
}*/