using fit.Data;
using fit.Models;
using fit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace fit.Pages.User;

public class WorkoutsModel(AppDbContext db, INotificationService notifications) : BasePageModel
{
    private const string TrainerPlanPrefix = "[Trainer Plan]";

    public List<WorkoutLog> Logs { get; set; } = [];
    public List<WorkoutLog> PlannedLogs { get; set; } = [];
    public Dictionary<int, string> PlannedVideoLinks { get; set; } = [];
    public List<WorkoutPlan> Plans { get; set; } = [];
    [BindProperty] public WorkoutLog Entry { get; set; } = new();
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public int TotalMinutes { get; set; }
    public int TotalCalories { get; set; }
    public string? PlanExplanation { get; set; }

    public async Task<IActionResult> OnGetAsync(string? dateFrom, string? dateTo)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        await LoadPageDataAsync(SessionUserId!.Value, dateFrom, dateTo);
        return Page();
    }

    public async Task<IActionResult> OnPostExplainPlanAsync(string? dateFrom, string? dateTo)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        await LoadPageDataAsync(SessionUserId!.Value, dateFrom, dateTo);

        if (!PlannedLogs.Any())
        {
            PlanExplanation = "No upcoming trainer plan found yet. Ask your trainer to generate your plan first.";
            return Page();
        }

        var first = PlannedLogs.First();
        var goalType = GetPlanGoalType(first.Notes);
        var timeline = GetPlanDurationWeeks(first.Notes);
        var next7Days = PlannedLogs.Take(7).ToList();
        var daysPlanned = next7Days.Count;
        var totalMinutes = next7Days.Sum(x => x.DurationMinutes);
        var totalCalories = next7Days.Sum(x => x.CaloriesBurned);

        PlanExplanation =
            $"This plan is focused on {goalType} for {timeline}. " +
            $"Your next {daysPlanned} planned day(s) include about {totalMinutes} total minutes and around {totalCalories} kcal target burn. " +
            $"Follow each day in order, use the video links for guidance, and mark days as done to track consistency.";

        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
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
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        var entry = await db.WorkoutLogs.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.WorkoutLogs.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Workout deleted.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostCompletePlanAsync(int id)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;

        var entry = await db.WorkoutLogs.FindAsync(id);
        if (entry == null || entry.UserId != uid) return Forbid();
        if (entry.Notes == null || !entry.Notes.StartsWith(TrainerPlanPrefix)) return RedirectToPage();

        entry.Notes = $"Completed trainer plan - {entry.Notes[TrainerPlanPrefix.Length..].Trim()}";
        if (entry.CaloriesBurned <= 0)
        {
            entry.CaloriesBurned = Math.Max(120, entry.DurationMinutes * 8);
        }

        await db.SaveChangesAsync();

        var owner = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == uid);
        if (owner?.TrainerId is int trainerId)
        {
            await notifications.NotifyUserAsync(
                trainerId,
                "Client completed training day",
                $"{owner.Name} completed today's assigned workout plan.",
                "/Trainer/Clients");
        }

        TempData["Message"] = "Great job! Daily trainer plan marked as completed.";
        return RedirectToPage();
    }

    public static string GetPlanSummary(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes)) return "-";
        var text = notes.Replace(TrainerPlanPrefix, "").Trim();
        var marker = "| Video:";
        var idx = text.IndexOf(marker, StringComparison.OrdinalIgnoreCase);
        if (idx >= 0) text = text[..idx].Trim();
        return text;
    }

    public static string GetPlanGoalType(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes)) return "-";
        var match = Regex.Match(notes, @"\(([^,]+),\s*\d+\s+weeks\)", RegexOptions.IgnoreCase);
        return match.Success ? match.Groups[1].Value.Trim() : "-";
    }

    public static string GetPlanDurationWeeks(string? notes)
    {
        if (string.IsNullOrWhiteSpace(notes)) return "-";
        var match = Regex.Match(notes, @",\s*(\d+)\s+weeks\)", RegexOptions.IgnoreCase);
        return match.Success ? $"{match.Groups[1].Value} weeks" : "-";
    }

    private static string? ExtractFirstUrl(string text)
    {
        var match = Regex.Match(text, @"https?://\S+", RegexOptions.IgnoreCase);
        return match.Success ? match.Value : null;
    }

    private async Task LoadPageDataAsync(int uid, string? dateFrom, string? dateTo)
    {
        DateFrom = dateFrom;
        DateTo = dateTo;
        Plans = await db.WorkoutPlans.OrderBy(p => p.Name).ToListAsync();

        var q = db.WorkoutLogs.Include(w => w.WorkoutPlan).Where(w => w.UserId == uid);
        if (DateTime.TryParse(dateFrom, out var df)) q = q.Where(w => w.Date >= df);
        if (DateTime.TryParse(dateTo, out var dt)) q = q.Where(w => w.Date <= dt);

        PlannedLogs = await q
            .Where(w => w.Notes != null && w.Notes.StartsWith(TrainerPlanPrefix) && w.Date >= DateTime.UtcNow.Date)
            .OrderBy(w => w.Date)
            .ToListAsync();

        PlannedVideoLinks = PlannedLogs
            .Where(w => !string.IsNullOrWhiteSpace(w.Notes))
            .Select(w => new { w.Id, Url = ExtractFirstUrl(w.Notes!) })
            .Where(x => !string.IsNullOrWhiteSpace(x.Url))
            .ToDictionary(x => x.Id, x => x.Url!);

        Logs = await q
            .Where(w => w.Notes == null || !w.Notes.StartsWith(TrainerPlanPrefix))
            .OrderByDescending(w => w.Date)
            .ToListAsync();

        TotalMinutes = Logs.Sum(w => w.DurationMinutes);
        TotalCalories = Logs.Sum(w => w.CaloriesBurned);
    }
}
