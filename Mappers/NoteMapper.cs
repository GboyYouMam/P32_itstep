using WebApplication1.Entities;
using WebApplication1.Models;

namespace WebApplication1.Mappers;

public class NoteMapper
{
    public static NoteEntity MapToEntity(NoteViewModel model, IEnumerable<TagEntity> tags)
    {
        if (model == null) throw new ArgumentNullException(nameof(model));

        return new NoteEntity
        {
            Title = model.Title,
            Content = model.Content,
            CreatedAt = model.CreatedAt,
            Tags = tags
                .Where(tag => model.TagsId.Contains(tag.Id))
                .ToList()
        };
    }
}