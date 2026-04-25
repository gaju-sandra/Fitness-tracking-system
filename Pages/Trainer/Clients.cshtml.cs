using fit.Data;
using fit.Models;
using fit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Trainer;

public class ClientsModel(AppDbContext db, INotificationService notifications) : BasePageModel
{
    private const string TrainerPlanPrefix = "[Trainer Plan]";

    private static readonly (string Title, int DurationMinutes, int Calories, string Requirements, string VideoUrl)[] WeeklyTemplate =
    [
        ("Day 1 – Cardio + Core", 55, 420, "Brisk walking/jogging 25 min; Jump rope 10 min; Plank 3x30s; Crunches 3x15; Leg raises 3x12", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 2 – Full Body Strength", 45, 360, "Squats 3x15; Push-ups 3x10; Lunges 3x12 each leg; Dumbbell rows 3x12; Glute bridges 3x15; Rest 30-60 sec", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 3 – Cardio HIIT", 40, 430, "30 sec sprint/fast jumps + 1 min walk/rest; Repeat 10-15 rounds; Cool down walk 10 min", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 4 – Active Recovery", 35, 180, "Light walking 20 min; Stretching/Yoga 15-20 min; Child's Pose + Downward Dog", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 5 – Lower Body + Cardio", 55, 440, "Squats 3x15; Step-ups 3x12; Lunges 3x12; Jogging or cycling 20-30 min", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 6 – Cardio + Core", 45, 350, "Fast walk/jog 30 min; Plank variations 10 min; Bicycle crunch 3x15", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
        ("Day 7 – Rest / Light Activity", 30, 120, "Optional light walk 20-30 min; Recovery focus", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7")
    ];

    public List<fit.Models.User> Clients { get; set; } = [];
    public Dictionary<int, BodyWeight?> LatestWeights { get; set; } = [];
    public Dictionary<int, int> WorkoutCounts { get; set; } = [];
    public Dictionary<int, List<Goal>> ActiveGoals { get; set; } = [];

    [BindProperty] public int PlanClientId { get; set; }
    [BindProperty] public int PlanGoalId { get; set; }
    [BindProperty] public string ClientEmail { get; set; } = "";

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Trainer"); if (r is RedirectToPageResult) return r;
        var uid = SessionUserId!.Value;
        Clients = await db.Users.Where(u => u.TrainerId == uid).ToListAsync();
        foreach (var c in Clients)
        {
            LatestWeights[c.Id] = await db.BodyWeights.Where(b => b.UserId == c.Id).OrderByDescending(b => b.Date).FirstOrDefaultAsync();
            WorkoutCounts[c.Id] = await db.WorkoutLogs.CountAsync(w => w.UserId == c.Id);
            ActiveGoals[c.Id] = await db.Goals
                .Where(g => g.UserId == c.Id && !g.IsCompleted && g.Deadline >= DateTime.UtcNow.Date)
                .OrderBy(g => g.Deadline)
                .ToListAsync();
        }
        return Page();
    }

    public async Task<IActionResult> OnPostCreatePlanAsync()
    {
        var r = RequireRole("Trainer"); if (r is RedirectToPageResult) return r;
        var trainerId = SessionUserId!.Value;
        var trainer = await db.Users.FirstOrDefaultAsync(u => u.Id == trainerId);
        if (trainer == null) return Forbid();

        var client = await db.Users.FirstOrDefaultAsync(u => u.Id == PlanClientId && u.TrainerId == trainerId);
        if (client == null) return Forbid();

        var goal = await db.Goals.FirstOrDefaultAsync(g => g.Id == PlanGoalId && g.UserId == PlanClientId && !g.IsCompleted);
        if (goal == null)
        {
            TempData["Message"] = "Goal not found for the selected client.";
            return RedirectToPage();
        }

        if (!string.Equals(goal.PreferredTrainerGender, "Any", StringComparison.OrdinalIgnoreCase)
            && !string.Equals(goal.PreferredTrainerGender, trainer.Gender, StringComparison.OrdinalIgnoreCase))
        {
            TempData["Message"] = $"This goal prefers a {goal.PreferredTrainerGender} trainer. Your profile gender is {trainer.Gender}.";
            return RedirectToPage();
        }

        var startDate = DateTime.UtcNow.Date;
        var endDate = goal.Deadline.Date;
        if (endDate < startDate)
        {
            TempData["Message"] = "Goal deadline has already passed.";
            return RedirectToPage();
        }

        var daysRemaining = Math.Max(1, (endDate - startDate).Days + 1);
        var durationWeeks = Math.Max(1, (int)Math.Ceiling(daysRemaining / 7.0));
        var normalizedType = NormalizeGoalType(goal.Type);

        var plan = await PickPlanForGoalAsync(normalizedType);
        if (plan == null)
        {
            TempData["Message"] = "No workout template exists. Ask admin to add workout plans first.";
            return RedirectToPage();
        }

        var weeklyTemplate = BuildWeeklyTemplateForGoal(normalizedType, durationWeeks);

        var difficultyScale = plan.Difficulty switch
        {
            "Beginner" => 0.9,
            "Intermediate" => 1.0,
            "Advanced" => 1.15,
            _ => 1.0
        };

        var created = 0;
        var dayCounter = 0;
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var template = weeklyTemplate[dayCounter % weeklyTemplate.Length];
            dayCounter++;

            var exists = await db.WorkoutLogs.AnyAsync(w =>
                w.UserId == PlanClientId &&
                w.Date.Date == date &&
                w.Notes != null &&
                w.Notes.StartsWith(TrainerPlanPrefix));

            if (exists)
            {
                continue;
            }

            db.WorkoutLogs.Add(new WorkoutLog
            {
                UserId = PlanClientId,
                Date = date,
                DurationMinutes = Math.Max(20, (int)Math.Round(template.DurationMinutes * difficultyScale)),
                CaloriesBurned = (int)Math.Round(template.Calories * difficultyScale),
                WorkoutPlanId = plan.Id,
                Notes = $"{TrainerPlanPrefix} {template.Title} | Goal: {goal.Title} ({normalizedType}, {durationWeeks} weeks) | {template.Requirements} | Video: {template.VideoUrl}"
            });
            created++;
        }

        await db.SaveChangesAsync();

        if (created > 0)
        {
            await notifications.NotifyUserAsync(
                PlanClientId,
                "New trainer plan generated",
                $"Your trainer created {created} daily workout(s) for your '{goal.Title}' goal.",
                "/User/Workouts");
        }

        TempData["Message"] = created > 0
            ? $"Created {created} daily workouts for {client.Name} based on {normalizedType} goal ({durationWeeks} weeks)."
            : "No new workout days were created. A trainer plan may already exist for this period.";

        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostAssignClientAsync()
    {
        var r = RequireRole("Trainer"); if (r is RedirectToPageResult) return r;
        var trainerId = SessionUserId!.Value;

        var email = (ClientEmail ?? string.Empty).Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["Message"] = "Enter a user email to assign.";
            return RedirectToPage();
        }

        var userRoleId = await db.Roles.Where(x => x.Name == "User").Select(x => x.Id).FirstOrDefaultAsync();
        if (userRoleId == 0)
        {
            TempData["Message"] = "User role is missing in configuration.";
            return RedirectToPage();
        }

        var client = await db.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email && u.RoleId == userRoleId);
        if (client == null)
        {
            TempData["Message"] = $"No user account found for {email}.";
            return RedirectToPage();
        }

        if (client.TrainerId.HasValue && client.TrainerId.Value != trainerId)
        {
            TempData["Message"] = $"{client.Name} is already assigned to another trainer.";
            return RedirectToPage();
        }

        client.TrainerId = trainerId;
        await db.SaveChangesAsync();

        await notifications.NotifyUserAsync(
            client.Id,
            "Trainer assigned",
            "You have been assigned a trainer. You can now receive workout plans and guidance.",
            "/User/Goals");

        TempData["Message"] = $"{client.Name} ({client.Email}) is now assigned to you. You can now view goals and generate plan.";
        return RedirectToPage();
    }

    private async Task<WorkoutPlan?> PickPlanForGoalAsync(string normalizedType)
    {
        var plans = await db.WorkoutPlans.OrderBy(p => p.Name).ToListAsync();
        if (plans.Count == 0) return null;

        string[] priorities = normalizedType switch
        {
            "Weight Loss" => ["HIIT", "Cardio", "Morning"],
            "Workout" => ["Strength", "Builder", "HIIT"],
            "Nutrition" => ["Morning", "Cardio", "Beginner"],
            _ => ["Cardio", "Strength"]
        };

        foreach (var key in priorities)
        {
            var match = plans.FirstOrDefault(p => p.Name.Contains(key, StringComparison.OrdinalIgnoreCase));
            if (match != null) return match;
        }

        return plans.First();
    }

    private static (string Title, int DurationMinutes, int Calories, string Requirements, string VideoUrl)[] BuildWeeklyTemplateForGoal(string normalizedType, int durationWeeks)
    {
        var isLongPlan = durationWeeks >= 12;

        return normalizedType switch
        {
            "Nutrition" =>
            [
                ("Day 1 – Light Cardio + Hydration", 35, 230, "Brisk walk 25 min; 2L water target; balanced plate guidance", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 2 – Mobility + Core", 30, 180, "Stretching 15 min; plank 3x25s; breathing drills", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 3 – Moderate Cardio", 40, 260, "Walk/jog intervals 30 min; cool down 10 min", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 4 – Recovery", 25, 120, "Easy walk and flexibility; sleep 7-8h", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 5 – Full Body Basics", 35, 240, "Bodyweight squats, wall pushups, glute bridges", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 6 – Cardio + Core", 35, 240, "Fast walk 25 min; bicycle crunch 3x12", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 7 – Rest", 20, 90, "Light activity only", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7")
            ],
            "Workout" =>
            [
                ("Day 1 – Strength Upper", isLongPlan ? 60 : 50, isLongPlan ? 430 : 360, "Push-ups, rows, shoulder press, core finish", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 2 – Strength Lower", isLongPlan ? 60 : 50, isLongPlan ? 450 : 370, "Squats, lunges, step-ups, calf raises", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 3 – Active Recovery", 30, 170, "Mobility and stretching", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 4 – Full Body", isLongPlan ? 65 : 55, isLongPlan ? 480 : 400, "Compound movements and circuit", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 5 – Conditioning", isLongPlan ? 50 : 40, isLongPlan ? 420 : 340, "Intervals and core", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 6 – Technique + Core", 40, 260, "Form-focused reps and planks", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7"),
                ("Day 7 – Rest", 20, 90, "Rest and recovery", "https://youtu.be/r_adSBYTkiU?si=KjA8b6WFAhONLDy7")
            ],
            _ => WeeklyTemplate
        };
    }

    private static string NormalizeGoalType(string? value)
    {
        var type = (value ?? string.Empty).Trim();
        return type switch
        {
            "Weight" => "Weight Loss",
            "Weight Loss" => "Weight Loss",
            "Nutrition" => "Nutrition",
            "Workout" => "Workout",
            _ => "Workout"
        };
    }
}
