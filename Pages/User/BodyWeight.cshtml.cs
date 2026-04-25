using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class BodyWeightModel(AppDbContext db) : BasePageModel
{
    public List<BodyWeight> Entries { get; set; } = [];
    [BindProperty] public BodyWeight Entry { get; set; } = new();
    public string ChartLabels { get; set; } = "[]";
    public string ChartData { get; set; } = "[]";
    public string? DateFrom { get; set; }
    public string? DateTo { get; set; }

    public async Task<IActionResult> OnGetAsync(string? dateFrom, string? dateTo)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        DateFrom = dateFrom; DateTo = dateTo;

        var q = db.BodyWeights.Where(b => b.UserId == uid);
        if (DateTime.TryParse(dateFrom, out var df)) q = q.Where(b => b.Date >= df);
        if (DateTime.TryParse(dateTo, out var dt)) q = q.Where(b => b.Date <= dt);
        Entries = await q.OrderByDescending(b => b.Date).ToListAsync();

        var chart = Entries.OrderBy(b => b.Date).ToList();
        ChartLabels = System.Text.Json.JsonSerializer.Serialize(chart.Select(b => b.Date.ToString("MMM d")));
        ChartData = System.Text.Json.JsonSerializer.Serialize(chart.Select(b => b.Weight));
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        Entry.UserId = SessionUserId!.Value;
        if (Entry.Id == 0) db.BodyWeights.Add(Entry);
        else
        {
            var existing = await db.BodyWeights.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            existing.Weight = Entry.Weight; existing.Date = Entry.Date; existing.Note = Entry.Note;
        }
        await db.SaveChangesAsync();
        TempData["Message"] = "Weight entry saved!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireRole("User"); if (r is RedirectToPageResult) return r;
        var entry = await db.BodyWeights.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.BodyWeights.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Entry deleted.";
        return RedirectToPage();
    }
}
