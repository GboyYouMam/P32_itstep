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
    
    public static TagEntity MapToEntity(TagViewModel tagViewModel, List<NoteEntity> notes)
    {
        if (tagViewModel == null) return null;

        return new TagEntity
        {
            Id = tagViewModel.Id,
            Name = tagViewModel.Name,
            Notes = notes.Where(note => tagViewModel.NotesId.Contains(note.Id)).ToList()
        };
    }
}