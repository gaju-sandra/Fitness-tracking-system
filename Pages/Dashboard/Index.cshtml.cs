using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Dashboard;

public class IndexModel(AppDbContext db) : BasePageModel
{
    public fit.Models.User? CurrentUser { get; set; }
    public List<BodyWeight> WeightHistory { get; set; } = [];
    public List<WorkoutLog> RecentWorkouts { get; set; } = [];
    public List<NutritionLog> TodayNutrition { get; set; } = [];
    public List<Goal> Goals { get; set; } = [];
    public List<Badge> Badges { get; set; } = [];
    public MoodLog? LatestMood { get; set; }
    public int Streak { get; set; }
    public float TodayCalories { get; set; }
    public float TodayCaloriesBurned { get; set; }
    // Admin stats
    public int TotalUsers { get; set; }
    public int TotalWorkoutLogs { get; set; }
    public int TotalFoods { get; set; }
    // Trainer stats
    public List<fit.Models.User> Clients { get; set; } = [];
    public string WeightChartData { get; set; } = "[]";
    public string WeightChartLabels { get; set; } = "[]";
    public string AiInsight { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var result = RequireLogin();
        if (result is RedirectToPageResult) return result;

        var uid = SessionUserId!.Value;

        if (IsAdmin)
        {
            TotalUsers = await db.Users.CountAsync();
            TotalWorkoutLogs = await db.WorkoutLogs.CountAsync();
            TotalFoods = await db.Foods.CountAsync();
            var recentUsers = await db.Users.Include(u => u.Role).OrderByDescending(u => u.CreatedAt).Take(5).ToListAsync();
            Clients = recentUsers;
        }
        else if (IsTrainer)
        {
            Clients = await db.Users.Include(u => u.Role)
                .Where(u => u.TrainerId == uid).ToListAsync();
            RecentWorkouts = await db.WorkoutLogs.Include(w => w.WorkoutPlan).Include(w => w.User)
                .Where(w => Clients.Select(c => c.Id).Contains(w.UserId))
                .OrderByDescending(w => w.Date).Take(10).ToListAsync();
        }
        else
        {
            CurrentUser = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == uid);
            WeightHistory = await db.BodyWeights.Where(b => b.UserId == uid)
                .OrderBy(b => b.Date).Take(10).ToListAsync();
            RecentWorkouts = await db.WorkoutLogs.Include(w => w.WorkoutPlan)
                .Where(w => w.UserId == uid).OrderByDescending(w => w.Date).Take(5).ToListAsync();
            var today = DateTime.UtcNow.Date;
            TodayNutrition = await db.NutritionLogs.Include(n => n.Food)
                .Where(n => n.UserId == uid && n.Date.Date == today).ToListAsync();
            Goals = await db.Goals.Where(g => g.UserId == uid).ToListAsync();
            Badges = await db.Badges.Where(b => b.UserId == uid).OrderByDescending(b => b.EarnedAt).Take(6).ToListAsync();
            LatestMood = await db.MoodLogs.Where(m => m.UserId == uid).OrderByDescending(m => m.Date).FirstOrDefaultAsync();

            TodayCalories = TodayNutrition.Sum(n => n.Food.CaloriesPer100g * n.Grams / 100);
            TodayCaloriesBurned = RecentWorkouts.Where(w => w.Date.Date == today).Sum(w => w.CaloriesBurned);

            // Streak calculation
            var logDates = await db.WorkoutLogs.Where(w => w.UserId == uid)
                .Select(w => w.Date.Date).Distinct().OrderByDescending(d => d).ToListAsync();
            var streak = 0;
            var check = DateTime.UtcNow.Date;
            foreach (var d in logDates)
            {
                if (d == check) { streak++; check = check.AddDays(-1); }
                else break;
            }
            Streak = streak;

            // Chart data
            WeightChartLabels = System.Text.Json.JsonSerializer.Serialize(WeightHistory.Select(w => w.Date.ToString("MMM d")));
            WeightChartData = System.Text.Json.JsonSerializer.Serialize(WeightHistory.Select(w => w.Weight));

            // AI insight
            AiInsight = GenerateInsight();
        }
        return Page();
    }

    private string GenerateInsight()
    {
        if (WeightHistory.Count >= 2)
        {
            var diff = WeightHistory.Last().Weight - WeightHistory.First().Weight;
            if (diff < 0) return $"Great progress! You've lost {Math.Abs(diff):F1}kg. Keep up the consistency! 🎉";
            if (diff > 0) return $"You've gained {diff:F1}kg. Consider adjusting your calorie intake and increasing cardio. 💡";
        }
        if (TodayCalories < 1200) return "Your calorie intake seems low today. Try adding a meal with local foods like Ibishyimbo or Ibirayi. 🥗";
        if (Streak >= 3) return $"Amazing! You're on a {Streak}-day workout streak! You're building a great habit. 🔥";
        return "Log your weight and workouts consistently to unlock personalized AI insights. 🤖";
    }
}
