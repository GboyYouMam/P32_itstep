using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class NoteViewModel
{
    [Required(ErrorMessage = "Title is required.")]
    [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
    [Display(Name = "Note Title", Description = "Enter your full name (2-50 characters).")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Content is required.")]
    [StringLength(300, ErrorMessage = "Content cannot exceed 300 characters.")]
    [Display(Name = "Content of the Note", Description = "Enter the content of your note (up to 300 characters).")]
    public string Content { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    
    [Required(ErrorMessage = "Tags are required.")]
    [Display(Name = "Tags", Description = "Add tags to categorize your note.")]
    public List<int> TagsId { get; set; } = new List<int>();
}