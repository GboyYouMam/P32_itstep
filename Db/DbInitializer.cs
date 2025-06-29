using WebApplication1.Entities;

namespace WebApplication1.Db;

public class DbInitializer
{
    public static void Seed(SqLiteDbContext context)
    {
        if (!context.Notes.Any() || !context.Tags.Any())
        {
            var tag1 = new TagEntity { Name = "Important"};
            var tag2 = new TagEntity { Name = "Work" };
            var tag3 = new TagEntity { Name = "Personal" };
            var tag4 = new TagEntity { Name = "Urgent" };
            context.Tags.AddRange(tag1, tag2, tag3, tag4);
            context.SaveChanges();
            
            var tags = context.Tags.ToList();
            
            var note1 = new NoteEntity
            {
                Title = "First Note",
                Content = "This is the content of the first note.",
                CreatedAt = DateTime.Now,
                Tags = new List<TagEntity>
                {
                    tags.Find(t => t.Name == "Important"),
                    tags.Find(t => t.Name == "Work"),
                    tags.Find(t => t.Name == "Urgent")
                }
            };

            var note2 = new NoteEntity
            {
                Title = "Second Note",
                Content = "This is the content of the second note.",
                CreatedAt = DateTime.Now,
                Tags = new List<TagEntity>
                {
                    tags.Find(t => t.Name == "Personal"),
                    tags.Find(t => t.Name == "Work")
                }
            };
            context.Notes.AddRange(note1, note2);
            context.SaveChanges();
        }
    }
}