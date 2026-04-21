using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Admin;

public class WorkoutPlansModel(AppDbContext db) : BasePageModel
{
    public List<WorkoutPlan> Plans { get; set; } = [];
    public List<fit.Models.User> Trainers { get; set; } = [];
    [BindProperty] public WorkoutPlan EditPlan { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Admin", "Trainer"); if (r is RedirectToPageResult) return r;
        Plans = await db.WorkoutPlans.OrderBy(p => p.Name).ToListAsync();
        Trainers = await db.Users.Where(u => u.RoleId == 2).ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("Admin", "Trainer"); if (r is RedirectToPageResult) return r;
        if (EditPlan.Id == 0) { EditPlan.CreatedByUserId = SessionUserId!.Value; db.WorkoutPlans.Add(EditPlan); }
        else db.WorkoutPlans.Update(EditPlan);
        await db.SaveChangesAsync();
        TempData["Message"] = "Workout plan saved.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireRole("Admin", "Trainer"); if (r is RedirectToPageResult) return r;
        var plan = await db.WorkoutPlans.FindAsync(id);
        if (plan != null) { db.WorkoutPlans.Remove(plan); await db.SaveChangesAsync(); }
        TempData["Message"] = "Plan deleted.";
        return RedirectToPage();
    }
}
