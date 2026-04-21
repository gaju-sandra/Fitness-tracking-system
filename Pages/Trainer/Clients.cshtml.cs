using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Trainer;

public class ClientsModel(AppDbContext db) : BasePageModel
{
    public List<fit.Models.User> Clients { get; set; } = [];
    public Dictionary<int, BodyWeight?> LatestWeights { get; set; } = [];
    public Dictionary<int, int> WorkoutCounts { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Trainer"); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        Clients = await db.Users.Where(u => u.TrainerId == uid).ToListAsync();
        foreach (var c in Clients)
        {
            LatestWeights[c.Id] = await db.BodyWeights.Where(b => b.UserId == c.Id).OrderByDescending(b => b.Date).FirstOrDefaultAsync();
            WorkoutCounts[c.Id] = await db.WorkoutLogs.CountAsync(w => w.UserId == c.Id);
        }
        return Page();
    }
}
