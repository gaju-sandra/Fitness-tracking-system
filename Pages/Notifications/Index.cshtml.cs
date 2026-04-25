using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Notifications;

public class IndexModel(AppDbContext db) : BasePageModel
{
    public List<Notification> Items { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        var role = SessionUserRole;

        Items = await db.Notifications
            .Where(n => (n.RecipientUserId.HasValue && n.RecipientUserId == uid)
                        || (!n.RecipientUserId.HasValue && n.RecipientRole == role))
            .OrderByDescending(n => n.CreatedAt)
            .Take(100)
            .ToListAsync();

        return Page();
    }

    public async Task<IActionResult> OnPostReadAsync(int id)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        var role = SessionUserRole;

        var item = await db.Notifications.FirstOrDefaultAsync(n => n.Id == id &&
            ((n.RecipientUserId.HasValue && n.RecipientUserId == uid)
             || (!n.RecipientUserId.HasValue && n.RecipientRole == role)));

        if (item == null) return RedirectToPage();

        item.IsRead = true;
        await db.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(item.Link)) return Redirect(item.Link);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostReadAllAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        var role = SessionUserRole;

        var items = await db.Notifications
            .Where(n => !n.IsRead &&
                        ((n.RecipientUserId.HasValue && n.RecipientUserId == uid)
                         || (!n.RecipientUserId.HasValue && n.RecipientRole == role)))
            .ToListAsync();

        foreach (var item in items) item.IsRead = true;
        await db.SaveChangesAsync();

        TempData["Message"] = "All notifications marked as read.";
        return RedirectToPage();
    }
}
