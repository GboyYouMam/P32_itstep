using Microsoft.AspNetCore.Identity;

namespace WebApplication1.Entities;

public class MyIdentityUserModel : IdentityUser
{
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public List<NoteEntity> Notes { get; set; } = new ();
    public List<TagEntity> Tags { get; set; } = new ();
}