# 📝 Notes Web Application

A web application for creating and managing personal notes with tag organization capabilities.

## 🚦 Getting Started

1. Clone the repository
2. Restore NuGet packages:
```bash
dotnet restore
```
3. Create and apply migrations:
```bash
# Create initial migration
dotnet ef migrations add InitialCreate

# If you need to add a new migration (e.g., for tags)
dotnet ef migrations add AddTags

# Apply migrations to the database
dotnet ef database update
```
4. (Optional) If you need to remove the last migration:
```bash
# Rollback the last migration
dotnet ef database update HEAD~1

# Remove last migration files
dotnet ef migrations remove
```
5. Run the project:
```bash
dotnet run
```
6. Open your browser and navigate to:
```
https://localhost:7066
```


## 🚀 Features

### Notes
- ✍️ Create, edit and delete notes
- 🔍 View note details
- 📄 Pagination for notes list
- 🔒 Personal notes for each user

### Tags
- 🏷️ Create and manage tags
- 🔗 Attach tags to notes
- 📊 View tag usage statistics
- 📄 Pagination for tags list

## 🛠️ Technologies

- ASP.NET Core MVC
- Entity Framework Core
- Microsoft SQL Server
- Bootstrap 5
- ASP.NET Core Identity

## 📦 Project Structure

```
notes-proj/
├── Controllers/           # MVC Controllers
├── Models/                # Data Models
├── Views/                 # Views
├── Db/                    # Database and EF Core context
├── Migrations/            # EF Core Migrations
└── wwwroot/              # Static files
```

## 👥 Author

- git @GboyYouMam
