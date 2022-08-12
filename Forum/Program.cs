using Forum;
using Forum.Middleware;
using Forum.Services;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllersWithViews();
//Заявление зависимости 
builder.Services.AddSingleton<RandomServices>();
builder.Services.AddSingleton<HttpRequestGuidCheck>();
//builder.Services.AddSingleton<IHasher, Md5Hasher>();
builder.Services.AddSingleton<IHasher, ShaHasher>();
//Universal
//builder.Services.AddSingleton<ITimeManage, dateTime>();
builder.Services.AddSingleton<ITimeManage, dateTimeFormat>();
builder.Services.AddScoped<DAO_Worker_Facade>();
builder.Services.AddScoped<IAuthService, SessionAuthService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddControllersWithViews().AddViewLocalization();
builder.Services.AddDbContext<Forum.DAL.Context.IntroContext>(options =>
         options.UseSqlServer(builder.Configuration.GetConnectionString("introDb")));
builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromHours(24);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseRequestLocalization(options =>
{
    options.SupportedCultures = new List<CultureInfo>
    {
        new CultureInfo("uk-UA"),
        new CultureInfo("ru-RU"),
        new CultureInfo("en-US")
    };
    options.SupportedUICultures = options.SupportedCultures;
    options.SetDefaultCulture(options.SupportedCultures[2].Name);
    options.RequestCultureProviders.Insert(0, new RouteDataRequestCultureProvider());
});
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseSession();
//Middleware
app.UseSessionAuth();
//app.UseMiddleware<ASP_Leson.Middleware.SessionAuthMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{culture=en-US}/{controller=Home}/{action=Index}/{id?}");
app.Run();

