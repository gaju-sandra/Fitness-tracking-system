using fit.Data;
using fit.Models;
using fit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class GoalsModel(AppDbContext db, INotificationService notifications) : BasePageModel
{
    private static readonly int[] AllowedDurations = [4, 8, 12, 16, 24];

    public List<Goal> Goals { get; set; } = [];
    [BindProperty] public Goal Entry { get; set; } = new();
    [BindProperty] public int WeeksToTarget { get; set; } = 8;
    [BindProperty] public bool PremiumWeeksPaid { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        Goals = await db.Goals.Where(g => g.UserId == SessionUserId!.Value).OrderBy(g => g.IsCompleted).ThenBy(g => g.Deadline).ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        if (!AllowedDurations.Contains(WeeksToTarget))
        {
            TempData["Message"] = "Please select one of: 4, 8, 12, 16, or 24 weeks.";
            return RedirectToPage();
        }

        if ((WeeksToTarget == 16 || WeeksToTarget == 24) && !PremiumWeeksPaid)
        {
            TempData["Message"] = "For 16 or 24 weeks goals, a 500 RWF payment is required. Please confirm payment to continue.";
            return RedirectToPage();
        }

        Entry.Type = NormalizeGoalType(Entry.Type);

        Entry.UserId = SessionUserId!.Value;
        Entry.PreferredTrainerGender = Entry.PreferredTrainerGender switch
        {
            "Male" => "Male",
            "Female" => "Female",
            _ => "Any"
        };
        Entry.Deadline = DateTime.UtcNow.Date.AddDays(WeeksToTarget * 7);
        var isNew = Entry.Id == 0;
        var becameCompleted = false;
        if (isNew) db.Goals.Add(Entry);
        else
        {
            var existing = await db.Goals.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            becameCompleted = !existing.IsCompleted && Entry.IsCompleted;
            existing.Title = Entry.Title; existing.Type = Entry.Type;
            existing.TargetValue = Entry.TargetValue; existing.CurrentValue = Entry.CurrentValue;
            existing.Deadline = Entry.Deadline; existing.IsCompleted = Entry.IsCompleted;
            existing.PreferredTrainerGender = Entry.PreferredTrainerGender;
        }
        await db.SaveChangesAsync();

        var owner = await db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == Entry.UserId);
        if (owner?.TrainerId is int trainerId)
        {
            if (isNew)
            {
                await notifications.NotifyUserAsync(
                    trainerId,
                    "Client created a new goal",
                    $"{owner.Name} created goal: {Entry.Title}",
                    "/Trainer/Clients");
            }
            else if (becameCompleted)
            {
                await notifications.NotifyUserAsync(
                    trainerId,
                    "Client completed a goal",
                    $"{owner.Name} marked goal '{Entry.Title}' as completed.",
                    "/Trainer/Clients");
            }
        }

        TempData["Message"] = "Goal saved!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        var entry = await db.Goals.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.Goals.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Goal deleted.";
        return RedirectToPage();
    }

    private static string NormalizeGoalType(string? value)
    {
        return (value ?? string.Empty).Trim() switch
        {
            "Weight" => "Weight Loss",
            "Weight Loss" => "Weight Loss",
            "Nutrition" => "Nutrition",
            "Workout" => "Workout",
            _ => "Workout"
        };
    }
}
