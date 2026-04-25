using fit.Data;
using fit.Models;
using Microsoft.EntityFrameworkCore;

namespace fit.Services;

public interface INotificationService
{
    Task NotifyUserAsync(int recipientUserId, string title, string message, string? link = null);
    Task NotifyRoleAsync(string role, string title, string message, string? link = null);
}

public class NotificationService(AppDbContext db) : INotificationService
{
    public async Task NotifyUserAsync(int recipientUserId, string title, string message, string? link = null)
    {
        db.Notifications.Add(new Notification
        {
            RecipientUserId = recipientUserId,
            Title = title,
            Message = message,
            Link = link,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        });

        await db.SaveChangesAsync();
    }

    public async Task NotifyRoleAsync(string role, string title, string message, string? link = null)
    {
        var roleName = (role ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(roleName)) return;

        var recipientRoleExists = await db.Roles.AnyAsync(r => r.Name == roleName);
        if (!recipientRoleExists) return;

        db.Notifications.Add(new Notification
        {
            RecipientRole = roleName,
            Title = title,
            Message = message,
            Link = link,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        });

        await db.SaveChangesAsync();
    }
}
