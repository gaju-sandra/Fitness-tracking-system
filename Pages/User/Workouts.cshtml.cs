using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class WorkoutsModel(AppDbContext db) : BasePageModel
{
    public List<WorkoutLog> Logs { get; set; } = [];
    public List<WorkoutPlan> Plans { get; set; } = [];
    [BindProperty] public WorkoutLog Entry { get; set; } = new();
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public int TotalMinutes { get; set; }
    public int TotalCalories { get; set; }

    public async Task<IActionResult> OnGetAsync(string? dateFrom, string? dateTo)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        DateFrom = dateFrom; DateTo = dateTo;
        Plans = await db.WorkoutPlans.OrderBy(p => p.Name).ToListAsync();

        var q = db.WorkoutLogs.Include(w => w.WorkoutPlan).Where(w => w.UserId == uid);
        if (DateTime.TryParse(dateFrom, out var df)) q = q.Where(w => w.Date >= df);
        if (DateTime.TryParse(dateTo, out var dt)) q = q.Where(w => w.Date <= dt);
        Logs = await q.OrderByDescending(w => w.Date).ToListAsync();
        TotalMinutes = Logs.Sum(w => w.DurationMinutes);
        TotalCalories = Logs.Sum(w => w.CaloriesBurned);
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        Entry.UserId = SessionUserId!.Value;
        if (Entry.Id == 0) db.WorkoutLogs.Add(Entry);
        else
        {
            var existing = await db.WorkoutLogs.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            existing.WorkoutPlanId = Entry.WorkoutPlanId; existing.Date = Entry.Date;
            existing.DurationMinutes = Entry.DurationMinutes; existing.CaloriesBurned = Entry.CaloriesBurned;
            existing.Notes = Entry.Notes;
        }
        await db.SaveChangesAsync();
        TempData["Message"] = "Workout logged!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var entry = await db.WorkoutLogs.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.WorkoutLogs.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Workout deleted.";
        return RedirectToPage();
    }
}
