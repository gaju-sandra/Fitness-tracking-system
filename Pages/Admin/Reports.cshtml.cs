using fit.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Admin;

public class ReportsModel(AppDbContext db) : BasePageModel
{
    public string UsersByRoleLabels { get; set; } = "[]";
    public string UsersByRoleData { get; set; } = "[]";
    public string WorkoutsByMonthLabels { get; set; } = "[]";
    public string WorkoutsByMonthData { get; set; } = "[]";
    public int TotalUsers { get; set; }
    public int TotalWorkouts { get; set; }
    public int TotalNutritionLogs { get; set; }
    public float AvgCaloriesPerDay { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;

        TotalUsers = await db.Users.CountAsync();
        TotalWorkouts = await db.WorkoutLogs.CountAsync();
        TotalNutritionLogs = await db.NutritionLogs.CountAsync();

        var roleGroups = await db.Users.Include(u => u.Role)
            .GroupBy(u => u.Role.Name)
            .Select(g => new { Role = g.Key, Count = g.Count() })
            .ToListAsync();
        UsersByRoleLabels = System.Text.Json.JsonSerializer.Serialize(roleGroups.Select(g => g.Role));
        UsersByRoleData = System.Text.Json.JsonSerializer.Serialize(roleGroups.Select(g => g.Count));

        var monthGroups = await db.WorkoutLogs
            .GroupBy(w => new { w.Date.Year, w.Date.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .Take(6).ToListAsync();
        WorkoutsByMonthLabels = System.Text.Json.JsonSerializer.Serialize(
            monthGroups.Select(g => $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Month)} {g.Year}"));
        WorkoutsByMonthData = System.Text.Json.JsonSerializer.Serialize(monthGroups.Select(g => g.Count));

        var nutritionLogs = await db.NutritionLogs.Include(n => n.Food).ToListAsync();
        if (nutritionLogs.Any())
        {
            var totalCals = nutritionLogs.Sum(n => n.Food.CaloriesPer100g * n.Grams / 100);
            var days = nutritionLogs.Select(n => n.Date.Date).Distinct().Count();
            AvgCaloriesPerDay = days > 0 ? totalCals / days : 0;
        }

        return Page();
    }
}
