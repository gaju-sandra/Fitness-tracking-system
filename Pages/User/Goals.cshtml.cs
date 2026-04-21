using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class GoalsModel(AppDbContext db) : BasePageModel
{
    public List<Goal> Goals { get; set; } = [];
    [BindProperty] public Goal Entry { get; set; } = new();
    [BindProperty] public int WeeksToTarget { get; set; } = 8;

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        Goals = await db.Goals.Where(g => g.UserId == SessionUserId!.Value).OrderBy(g => g.IsCompleted).ThenBy(g => g.Deadline).ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        if (WeeksToTarget is < 1 or > 52)
        {
            TempData["Message"] = "Please select a timeline between 1 and 52 weeks.";
            return RedirectToPage();
        }

        Entry.UserId = SessionUserId!.Value;
        Entry.Deadline = DateTime.UtcNow.Date.AddDays(WeeksToTarget * 7);
        if (Entry.Id == 0) db.Goals.Add(Entry);
        else
        {
            var existing = await db.Goals.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            existing.Title = Entry.Title; existing.Type = Entry.Type;
            existing.TargetValue = Entry.TargetValue; existing.CurrentValue = Entry.CurrentValue;
            existing.Deadline = Entry.Deadline; existing.IsCompleted = Entry.IsCompleted;
        }
        await db.SaveChangesAsync();
        TempData["Message"] = "Goal saved!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var entry = await db.Goals.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.Goals.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Goal deleted.";
        return RedirectToPage();
    }
}
