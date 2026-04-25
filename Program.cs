using fit.Data;
using fit.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning)));
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromHours(8);
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.ExecuteSqlRawAsync(
        """
        IF OBJECT_ID(N'dbo.Notifications', N'U') IS NULL
        BEGIN
            CREATE TABLE dbo.Notifications
            (
                Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
                RecipientUserId INT NULL,
                RecipientRole NVARCHAR(50) NULL,
                Title NVARCHAR(200) NOT NULL,
                Message NVARCHAR(1000) NOT NULL,
                Link NVARCHAR(300) NULL,
                IsRead BIT NOT NULL CONSTRAINT DF_Notifications_IsRead DEFAULT(0),
                CreatedAt DATETIME2 NOT NULL CONSTRAINT DF_Notifications_CreatedAt DEFAULT(SYSUTCDATETIME()),
                CONSTRAINT FK_Notifications_Users_RecipientUserId FOREIGN KEY (RecipientUserId) REFERENCES dbo.Users(Id) ON DELETE CASCADE
            );

            CREATE INDEX IX_Notifications_RecipientUserId_IsRead_CreatedAt ON dbo.Notifications(RecipientUserId, IsRead, CreatedAt);
        END;
        """);
}

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
