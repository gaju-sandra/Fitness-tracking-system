using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Admin;

public class FoodsModel(AppDbContext db) : BasePageModel
{
    public List<Food> Foods { get; set; } = [];
    [BindProperty] public Food EditFood { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Admin", "Trainer"); if (r is RedirectToPageResult) return r;
        Foods = await db.Foods.OrderBy(f => f.Category).ThenBy(f => f.Name).ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;
        if (EditFood.Id == 0) db.Foods.Add(EditFood);
        else db.Foods.Update(EditFood);
        await db.SaveChangesAsync();
        TempData["Message"] = "Food saved.";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;
        var food = await db.Foods.FindAsync(id);
        if (food != null) { db.Foods.Remove(food); await db.SaveChangesAsync(); }
        TempData["Message"] = "Food deleted.";
        return RedirectToPage();
    }
}
