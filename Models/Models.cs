namespace fit.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<User> Users { get; set; } = [];
}

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public int? TrainerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<BodyWeight> BodyWeights { get; set; } = [];
    public ICollection<Goal> Goals { get; set; } = [];
    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = [];
    public ICollection<NutritionLog> NutritionLogs { get; set; } = [];
    public ICollection<MoodLog> MoodLogs { get; set; } = [];
    public ICollection<Badge> Badges { get; set; } = [];
}

public class BodyWeight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public float Weight { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}

public class Goal
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Title { get; set; } = "";
    public string Type { get; set; } = "Weight";
    public float TargetValue { get; set; }
    public float CurrentValue { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
}

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public float CaloriesPer100g { get; set; }
    public float ProteinPer100g { get; set; }
    public float CarbsPer100g { get; set; }
    public float FatPer100g { get; set; }
    public bool IsLocal { get; set; }
}

public class NutritionLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int FoodId { get; set; }
    public Food Food { get; set; } = null!;
    public float Grams { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string MealType { get; set; } = "Lunch";
}

public class WorkoutPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Difficulty { get; set; } = "Beginner";
    public int CreatedByUserId { get; set; }
    public ICollection<WorkoutLog> Logs { get; set; } = [];
}

public class WorkoutLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int WorkoutPlanId { get; set; }
    public WorkoutPlan WorkoutPlan { get; set; } = null!;
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public int DurationMinutes { get; set; }
    public int CaloriesBurned { get; set; }
    public string? Notes { get; set; }
}

public class MoodLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int MoodScore { get; set; }
    public int EnergyScore { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}

public class Badge
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "🏅";
    public string Description { get; set; } = "";
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
}
