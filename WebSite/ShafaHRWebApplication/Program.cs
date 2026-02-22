using ShafaHRCoreLib.Helpers;
using ShafaHRCoreLib.Managers;
using ShafaHRCoreLib.Models;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EFContext>(options =>
    options.UseLazyLoadingProxies()
           .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

// فعال کردن Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 500 * 1024 * 1024; // 500MB
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 600 * 1024 * 1024;
});


var app = builder.Build();

app.UseStaticFiles();

// مقداردهی HttpContextProvider
HttpContextProvider.Configure(app.Services.GetRequiredService<IHttpContextAccessor>());


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSession();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapDefaultControllerRoute();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


using (var context = new EFContext(new DbContextOptionsBuilder<EFContext>().UseLazyLoadingProxies().UseSqlServer(CommonFunctions.ConnectionString).Options))
{
    context.Database.Migrate();
}



using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EFContext>();
    db.Database.Migrate();  
}

app.Run();
