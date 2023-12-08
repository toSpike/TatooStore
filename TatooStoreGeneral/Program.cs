using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TatooStore.Services;
using TatooStore.Services.Repository;
using TatooStore.Services.DbInitializer;
using TatooStore.Services.Repository.AccountRepository;
using TatooStore.Models.DataModels;
using TatooStore.Services.Repository.GoodsRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddMvcCore();
builder.Services.AddDbContextPool<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionString"));
});
builder.Services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();


builder.Services.AddScoped<IGoodsRepository, GoodsRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IDbInitializer, DbInitializer>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Account/Login";
});

var app = builder.Build();


/*using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await Init.AdminInitAsync(services);
}*/

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
SeedDatabase();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbInitializer.Initialize();
    }
}