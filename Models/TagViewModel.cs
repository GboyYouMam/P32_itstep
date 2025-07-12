using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

public class TagViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Tag name is required.")]
    [StringLength(50, ErrorMessage = "Tag name cannot exceed 50 characters.")]
    [Display(Name = "Tag Name", Description = "Enter the name of the tag (up to 50 characters).")]
    public string Name { get; set; }
    
    [Display(Name = "Notes", Description = "List of notes associated with this tag.")]
    public List<int> NotesId { get; set; } = new List<int>();
}