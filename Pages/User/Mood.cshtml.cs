using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.User;

public class MoodModel(AppDbContext db) : BasePageModel
{
    public List<MoodLog> Logs { get; set; } = [];
    [BindProperty] public MoodLog Entry { get; set; } = new();
    public string ChartLabels { get; set; } = "[]";
    public string MoodData { get; set; } = "[]";
    public string EnergyData { get; set; } = "[]";
    public string Recommendation { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        Logs = await db.MoodLogs.Where(m => m.UserId == uid).OrderByDescending(m => m.Date).Take(14).ToListAsync();
        var chart = Logs.OrderBy(m => m.Date).ToList();
        ChartLabels = System.Text.Json.JsonSerializer.Serialize(chart.Select(m => m.Date.ToString("MMM d")));
        MoodData = System.Text.Json.JsonSerializer.Serialize(chart.Select(m => m.MoodScore));
        EnergyData = System.Text.Json.JsonSerializer.Serialize(chart.Select(m => m.EnergyScore));

        var latest = Logs.FirstOrDefault();
        Recommendation = latest switch
        {
            { MoodScore: <= 2, EnergyScore: <= 2 } => "You seem tired and low. Consider a rest day, light stretching, and hydration. 💧",
            { MoodScore: >= 4, EnergyScore: >= 4 } => "You're feeling great! Perfect day for a high-intensity workout. 🔥",
            { EnergyScore: <= 2 } => "Low energy today. Try a 20-min walk and eat energy-rich foods like Ibijumba. 🍠",
            { MoodScore: <= 2 } => "Mood is low. Light exercise and social activity can help boost it. 😊",
            _ => "You're doing well! Keep up your routine and stay consistent. 💪"
        };
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        Entry.UserId = SessionUserId!.Value;
        if (Entry.Id == 0) db.MoodLogs.Add(Entry);
        else
        {
            var existing = await db.MoodLogs.FindAsync(Entry.Id);
            if (existing == null || existing.UserId != Entry.UserId) return Forbid();
            existing.MoodScore = Entry.MoodScore; existing.EnergyScore = Entry.EnergyScore;
            existing.Date = Entry.Date; existing.Note = Entry.Note;
        }
        await db.SaveChangesAsync();
        TempData["Message"] = "Mood logged!";
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireLogin(); if (r is RedirectToPageResult) return r;
        var entry = await db.MoodLogs.FindAsync(id);
        if (entry != null && entry.UserId == SessionUserId!.Value)
        { db.MoodLogs.Remove(entry); await db.SaveChangesAsync(); }
        TempData["Message"] = "Entry deleted.";
        return RedirectToPage();
    }
}
