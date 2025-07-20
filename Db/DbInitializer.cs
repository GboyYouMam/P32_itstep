using Microsoft.AspNetCore.Identity;
using WebApplication1.Db;
using WebApplication1.Entities;

public class DbInitializer
{
    public static void Seed(SqLiteDbContext context, UserManager<MyIdentityUserModel> userManager)
    {
        context.Database.EnsureCreated();

        if (!context.Users.Any())
        {
            var user = new MyIdentityUserModel
            {
                UserName = "testuser@example.com",
                Email = "testuser@example.com",
                EmailConfirmed = true
            };

            var result = userManager.CreateAsync(user, "Test123!").Result;
            if (!result.Succeeded)
                throw new Exception("Failed to create seed user.");
        }

        var seededUser = context.Users.First(u => u.Email == "testuser@example.com");

        if (!context.Notes.Any() && !context.Tags.Any())
        {
            var tag1 = new TagEntity { Name = "Important", UserId = seededUser.Id };
            var tag2 = new TagEntity { Name = "Work", UserId = seededUser.Id };
            var tag3 = new TagEntity { Name = "Personal", UserId = seededUser.Id };
            var tag4 = new TagEntity { Name = "Urgent", UserId = seededUser.Id };

            context.Tags.AddRange(tag1, tag2, tag3, tag4);
            context.SaveChanges();

            var tags = context.Tags.Where(t => t.UserId == seededUser.Id).ToList();

            var note1 = new NoteEntity
            {
                Title = "First Note",
                Content = "This is the content of the first note.",
                CreatedAt = DateTime.Now,
                UserId = seededUser.Id,
                Tags = new List<TagEntity>
                {
                    tags.FirstOrDefault(t => t.Name == "Important"),
                    tags.FirstOrDefault(t => t.Name == "Work"),
                    tags.FirstOrDefault(t => t.Name == "Urgent")
                }
            };

            var note2 = new NoteEntity
            {
                Title = "Second Note",
                Content = "This is the content of the second note.",
                CreatedAt = DateTime.Now,
                UserId = seededUser.Id,
                Tags = new List<TagEntity>
                {
                    tags.FirstOrDefault(t => t.Name == "Personal"),
                    tags.FirstOrDefault(t => t.Name == "Work")
                }
            };

            context.Notes.AddRange(note1, note2);
            context.SaveChanges();
        }
    }
}
