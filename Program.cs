using Microsoft.AspNetCore.Identity;
using WebApplication1.Db;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SqLiteDbContext>(options =>
    options.UseSqlite("Data Source=notes.db"));

builder.Services.AddDefaultIdentity<MyIdentityUserModel>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Добавлено для поддержки ролей
    .AddEntityFrameworkStores<SqLiteDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SqLiteDbContext>();
    db.Database.Migrate();
    db.Database.EnsureCreated();
}

// Инициализация базы данных с начальными данными
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SqLiteDbContext>();
    var userManager = services.GetRequiredService<UserManager<MyIdentityUserModel>>();
    DbInitializer.Seed(context, userManager);
}



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Забув?

app.UseRouting();

app.UseAuthentication();    // <--- Обов'язково
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();



app.Run();
