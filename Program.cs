using fit.Data;
using fit.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value ?? string.Empty;
    var isPublicPath =
        path.Equals("/", StringComparison.OrdinalIgnoreCase) ||
        path.Equals("/Index", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Auth/Login", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Auth/Register", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Auth/TwoFactor", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Auth/Logout", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/Error", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/css", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/js", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/lib", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/images", StringComparison.OrdinalIgnoreCase) ||
        path.StartsWith("/favicon", StringComparison.OrdinalIgnoreCase);

    var isLoggedIn = context.Session.GetInt32("UserId").HasValue;
    if (!isPublicPath && !isLoggedIn)
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }

    await next();
});

app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
