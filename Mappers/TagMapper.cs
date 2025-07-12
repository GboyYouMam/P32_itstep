using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers;

public class TagMapper
{
    public static TagViewModel MapToViewModel(TagEntity tagEntity)
    {
        if (tagEntity == null) return null;

        return new TagViewModel
        {
            Id = tagEntity.Id,
            Name = tagEntity.Name,
            NotesId = tagEntity.Notes.Select(n => n.Id).ToList()
        };
    }
}