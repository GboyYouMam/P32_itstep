using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

public class TagEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Tag name is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Tag name cannot exceed 50 characters.")]
    public string Name { get; set; }
    
    public string UserId { get; set; }  // Foreign Key
    public MyIdentityUserModel User { get; set; } // Navigation property

    
    public ICollection<NoteEntity> Notes { get; set; } = new List<NoteEntity>();
    
    public string GetNameById(int id)
    {
        return Notes.FirstOrDefault(n => n.Id == id)?.Title ?? "Unknown Note";
    }
}