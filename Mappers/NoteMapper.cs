using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers;

public class NoteMapper
{
    
    public static NoteEntity MapToEntity(NoteEntity entity, NoteViewModel model, List<TagEntity> tags)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        if (entity == null) throw new ArgumentNullException(nameof(entity));
        
        entity.Title = model.Title;
        entity.Content = model.Content;
        entity.Tags = tags;
        
        return entity;
    }
    public static NoteEntity MapToEntity(NoteViewModel model, IEnumerable<TagEntity> tags)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        return new NoteEntity
        {
            Id = model.Id,
            Title = model.Title,
            Content = model.Content,
            Tags = tags.Where(tag => model.TagsId.Contains(tag.Id)).ToList(),
        };
    }
    
    public static NoteViewModel MapToViewModel(NoteEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new NoteViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            TagsId = entity.Tags.Select(tag => tag.Id).ToList()
        };
    }
}