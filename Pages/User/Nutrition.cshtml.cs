using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class NutritionModel(AppDbContext db) : BasePageModel
{
    public List<NutritionLog> Logs { get; set; } = [];
    public List<Food> Foods { get; set; } = [];
    [BindProperty] public NutritionLog Entry { get; set; } = new();
    public float TotalCalories { get; set; }
    public float TotalProtein { get; set; }
    public string? DateFilter { get; set; }
    public string MacroLabels { get; set; } = "[]";
    public string MacroData { get; set; } = "[]";

    public async Task<IActionResult> OnGetAsync(string? date)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        DateFilter = date ?? DateTime.UtcNow.ToString("yyyy-MM-dd");
        Foods = await db.Foods.OrderBy(f => f.Name).ToListAsync();

        var filterDate = DateTime.TryParse(DateFilter, out var d) ? d.Date : DateTime.UtcNow.Date;
        Logs = await db.NutritionLogs.Include(n => n.Food)
            .Where(n => n.UserId == uid && n.Date.Date == filterDate)
            .OrderBy(n => n.MealType).ToListAsync();

        TotalCalories = Logs.Sum(n => n.Food.CaloriesPer100g * n.Grams / 100);
        TotalProtein = Logs.Sum(n => n.Food.ProteinPer100g * n.Grams / 100);
        var totalCarbs = Logs.Sum(n => n.Food.CarbsPer100g * n.Grams / 100);
        var totalFat = Logs.Sum(n => n.Food.FatPer100g * n.Grams / 100);

        MacroLabels = System.Text.Json.JsonSerializer.Serialize(new[] { "Protein", "Carbs", "Fat" });
        MacroData = System.Text.Json.JsonSerializer.Serialize(new[] { Math.Round(TotalProtein, 1), Math.Round(totalCarbs, 1), Math.Round(totalFat, 1) });
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        Entry.UserId = SessionUserId!.Value;
        if (Entry.Id == 0) db.NutritionLogs.Add(Entry);
        else
        {
            var existing = await db.NutritionLogs.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            existing.FoodId = Entry.FoodId; existing.Grams = Entry.Grams;
            existing.Date = Entry.Date; existing.MealType = Entry.MealType;
        }
        await db.SaveChangesAsync();
        TempData["Message"] = "Nutrition entry saved!";
        return RedirectToPage(new { date = Entry.Date.ToString("yyyy-MM-dd") });
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var entry = await db.NutritionLogs.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.NutritionLogs.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Entry deleted.";
        return RedirectToPage();
    }
}
