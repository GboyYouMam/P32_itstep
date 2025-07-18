using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Entities;

public class ProfileUserEntity
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    public string MyIdentityUserId { get; set; }

    public virtual MyIdentityUserModel User { get; set; }

    [Required]
    [StringLength(100)]
    public string UserName { get; set; }

    public virtual List<TagEntity> Tags { get; set; } = new();
    public virtual List<NoteEntity> Notes { get; set; } = new();
}