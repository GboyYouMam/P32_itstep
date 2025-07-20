using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

public class NoteEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("Id")]  
    public int Id { get; set; }
    
    [Required]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
    public string Title { get; set; }
    
    
    public string UserId { get; set; }  // Foreign Key
    public MyIdentityUserModel User { get; set; } // Navigation property

    
    [Required]
    [StringLength(300, MinimumLength = 5, ErrorMessage = "Content must be between 5 and 300 characters.")]
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }

    public ICollection<TagEntity> Tags { get; set; } = new List<TagEntity>();
}