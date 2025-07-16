using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebApplication1.Entities;

namespace WebApplication1.Db;
using Microsoft.EntityFrameworkCore;

public class SqLiteDbContext : IdentityDbContext
{
    public DbSet<TagEntity> Tags { get; set; }
    public DbSet<NoteEntity> Notes { get; set; }
    
    public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options)
    {
    }
}