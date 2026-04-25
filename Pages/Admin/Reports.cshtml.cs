using fit.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace fit.Pages.Admin;

public class ReportsModel(AppDbContext db) : BasePageModel
{
    public class ActivityRow
    {
        public DateTime Date { get; set; }
        public string UserName { get; set; } = "";
        public string Role { get; set; } = "";
        public string Category { get; set; } = "";
        public string Item { get; set; } = "";
        public float Value { get; set; }
    }

    public string UsersByRoleLabels { get; set; } = "[]";
    public string UsersByRoleData { get; set; } = "[]";
    public string WorkoutsByMonthLabels { get; set; } = "[]";
    public string WorkoutsByMonthData { get; set; } = "[]";
    public int TotalUsers { get; set; }
    public int TotalWorkouts { get; set; }
    public int TotalNutritionLogs { get; set; }
    public float AvgCaloriesPerDay { get; set; }
    public string? RoleFilter { get; set; }
    public string? NameFilter { get; set; }
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }
    public List<ActivityRow> ActivityRows { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(string? role, string? name, string? dateFrom, string? dateTo)
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;

        await LoadReportDataAsync(role, name, dateFrom, dateTo);
        return Page();
    }

    public async Task<IActionResult> OnGetExportExcelAsync(string? role, string? name, string? dateFrom, string? dateTo)
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;

        await LoadReportDataAsync(role, name, dateFrom, dateTo);

        var sb = new StringBuilder();
        sb.AppendLine("Section\tLabel\tValue");
        sb.AppendLine($"Summary\tTotal Users\t{TotalUsers}");
        sb.AppendLine($"Summary\tWorkout Sessions\t{TotalWorkouts}");
        sb.AppendLine($"Summary\tNutrition Logs\t{TotalNutritionLogs}");
        sb.AppendLine($"Summary\tAvg kcal/day\t{AvgCaloriesPerDay:F0}");

        sb.AppendLine();
        sb.AppendLine("Recent Activity\tDate\tUser\tRole\tCategory\tItem\tValue");
        foreach (var row in ActivityRows)
        {
            sb.AppendLine($"Activity\t{row.Date:yyyy-MM-dd}\t{row.UserName}\t{row.Role}\t{row.Category}\t{row.Item}\t{row.Value:F0}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "application/vnd.ms-excel", "fittrack-report.xls");
    }

    private async Task LoadReportDataAsync(string? role, string? name, string? dateFrom, string? dateTo)
    {
        RoleFilter = string.IsNullOrWhiteSpace(role) ? null : role;
        NameFilter = string.IsNullOrWhiteSpace(name) ? null : name.Trim();
        DateFrom = dateFrom;
        DateTo = dateTo;

        var usersQuery = db.Users.Include(u => u.Role).AsQueryable();
        if (!string.IsNullOrWhiteSpace(RoleFilter))
            usersQuery = usersQuery.Where(u => u.Role.Name == RoleFilter);
        if (!string.IsNullOrWhiteSpace(NameFilter))
            usersQuery = usersQuery.Where(u => u.Name.Contains(NameFilter));

        var workoutQuery = db.WorkoutLogs
            .Include(w => w.User).ThenInclude(u => u.Role)
            .Include(w => w.WorkoutPlan)
            .AsQueryable();

        var nutritionQuery = db.NutritionLogs
            .Include(n => n.User).ThenInclude(u => u.Role)
            .Include(n => n.Food)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(RoleFilter))
        {
            workoutQuery = workoutQuery.Where(w => w.User.Role.Name == RoleFilter);
            nutritionQuery = nutritionQuery.Where(n => n.User.Role.Name == RoleFilter);
        }

        if (!string.IsNullOrWhiteSpace(NameFilter))
        {
            workoutQuery = workoutQuery.Where(w => w.User.Name.Contains(NameFilter));
            nutritionQuery = nutritionQuery.Where(n => n.User.Name.Contains(NameFilter));
        }

        if (DateTime.TryParse(dateFrom, out var fromDate))
        {
            var from = fromDate.Date;
            workoutQuery = workoutQuery.Where(w => w.Date >= from);
            nutritionQuery = nutritionQuery.Where(n => n.Date >= from);
        }

        if (DateTime.TryParse(dateTo, out var toDate))
        {
            var toExclusive = toDate.Date.AddDays(1);
            workoutQuery = workoutQuery.Where(w => w.Date < toExclusive);
            nutritionQuery = nutritionQuery.Where(n => n.Date < toExclusive);
        }

        TotalUsers = await usersQuery.CountAsync();
        TotalWorkouts = await workoutQuery.CountAsync();
        TotalNutritionLogs = await nutritionQuery.CountAsync();

        var roleGroups = await usersQuery
            .GroupBy(u => u.Role.Name)
            .Select(g => new { Role = g.Key, Count = g.Count() })
            .ToListAsync();
        UsersByRoleLabels = System.Text.Json.JsonSerializer.Serialize(roleGroups.Select(g => g.Role));
        UsersByRoleData = System.Text.Json.JsonSerializer.Serialize(roleGroups.Select(g => g.Count));

        var monthGroups = await workoutQuery
            .GroupBy(w => new { w.Date.Year, w.Date.Month })
            .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
            .OrderBy(g => g.Year).ThenBy(g => g.Month)
            .Take(6).ToListAsync();
        WorkoutsByMonthLabels = System.Text.Json.JsonSerializer.Serialize(
            monthGroups.Select(g => $"{System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Month)} {g.Year}"));
        WorkoutsByMonthData = System.Text.Json.JsonSerializer.Serialize(monthGroups.Select(g => g.Count));

        var nutritionLogs = await nutritionQuery.ToListAsync();
        if (nutritionLogs.Any())
        {
            var totalCals = nutritionLogs.Sum(n => n.Food.CaloriesPer100g * n.Grams / 100);
            var days = nutritionLogs.Select(n => n.Date.Date).Distinct().Count();
            AvgCaloriesPerDay = days > 0 ? totalCals / days : 0;
        }

        var workoutRows = await workoutQuery
            .Select(w => new ActivityRow
            {
                Date = w.Date,
                UserName = w.User.Name,
                Role = w.User.Role.Name,
                Category = "Workout",
                Item = w.WorkoutPlan.Name,
                Value = w.CaloriesBurned
            })
            .ToListAsync();

        var nutritionRows = await nutritionQuery
            .Select(n => new ActivityRow
            {
                Date = n.Date,
                UserName = n.User.Name,
                Role = n.User.Role.Name,
                Category = "Nutrition",
                Item = n.Food.Name + " (" + n.MealType + ")",
                Value = n.Food.CaloriesPer100g * n.Grams / 100
            })
            .ToListAsync();

        ActivityRows = workoutRows
            .Concat(nutritionRows)
            .OrderByDescending(a => a.Date)
            .Take(40)
            .ToList();
    }
}
