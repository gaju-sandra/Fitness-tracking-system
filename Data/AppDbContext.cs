using fit.Models;
using Microsoft.EntityFrameworkCore;

namespace fit.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<BodyWeight> BodyWeights => Set<BodyWeight>();
    public DbSet<Goal> Goals => Set<Goal>();
    public DbSet<Food> Foods => Set<Food>();
    public DbSet<NutritionLog> NutritionLogs => Set<NutritionLog>();
    public DbSet<WorkoutPlan> WorkoutPlans => Set<WorkoutPlan>();
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();
    public DbSet<MoodLog> MoodLogs => Set<MoodLog>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Location>()
            .HasIndex(l => new { l.Province, l.Sector })
            .IsUnique();

        b.Entity<Notification>()
            .HasIndex(n => new { n.RecipientUserId, n.IsRead, n.CreatedAt });

        b.Entity<Notification>()
            .HasOne(n => n.RecipientUser)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.RecipientUserId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "Trainer" },
            new Role { Id = 3, Name = "User" }
        );

        b.Entity<User>().HasData(
            new User { Id = 1, Name = "Admin User", Email = "admin@fittrack.rw", Gender = "Male", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"), RoleId = 1, CreatedAt = new DateTime(2025, 1, 1) },
            new User { Id = 2, Name = "Jean Trainer", Email = "trainer@fittrack.rw", Gender = "Male", PasswordHash = BCrypt.Net.BCrypt.HashPassword("Trainer@123"), RoleId = 2, CreatedAt = new DateTime(2025, 1, 1) },
            new User { Id = 3, Name = "Alice Uwase", Email = "alice@fittrack.rw", Gender = "Female", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"), RoleId = 3, TrainerId = 2, CreatedAt = new DateTime(2025, 1, 1) },
            new User { Id = 4, Name = "Bob Mugisha", Email = "bob@fittrack.rw", Gender = "Male", PasswordHash = BCrypt.Net.BCrypt.HashPassword("User@123"), RoleId = 3, TrainerId = 2, CreatedAt = new DateTime(2025, 1, 1) }
        );

        b.Entity<Food>().HasData(
            new Food { Id = 1, Name = "Ugali", Category = "Grains", CaloriesPer100g = 360, ProteinPer100g = 3, CarbsPer100g = 79, FatPer100g = 1, IsLocal = true },
            new Food { Id = 2, Name = "Isombe (Cassava Leaves)", Category = "Vegetables", CaloriesPer100g = 45, ProteinPer100g = 4, CarbsPer100g = 6, FatPer100g = 1, IsLocal = true },
            new Food { Id = 3, Name = "Ibirayi (Potatoes)", Category = "Vegetables", CaloriesPer100g = 77, ProteinPer100g = 2, CarbsPer100g = 17, FatPer100g = 0.1f, IsLocal = true },
            new Food { Id = 4, Name = "Beans (Ibishyimbo)", Category = "Legumes", CaloriesPer100g = 347, ProteinPer100g = 22, CarbsPer100g = 63, FatPer100g = 1, IsLocal = true },
            new Food { Id = 5, Name = "Chicken Breast", Category = "Protein", CaloriesPer100g = 165, ProteinPer100g = 31, CarbsPer100g = 0, FatPer100g = 3.6f, IsLocal = false },
            new Food { Id = 6, Name = "Banana (Igitoki)", Category = "Fruits", CaloriesPer100g = 89, ProteinPer100g = 1.1f, CarbsPer100g = 23, FatPer100g = 0.3f, IsLocal = true },
            new Food { Id = 7, Name = "Milk (Amata)", Category = "Dairy", CaloriesPer100g = 61, ProteinPer100g = 3.2f, CarbsPer100g = 4.8f, FatPer100g = 3.3f, IsLocal = true },
            new Food { Id = 8, Name = "Sweet Potato (Ibijumba)", Category = "Vegetables", CaloriesPer100g = 86, ProteinPer100g = 1.6f, CarbsPer100g = 20, FatPer100g = 0.1f, IsLocal = true }
        );

        b.Entity<WorkoutPlan>().HasData(
            new WorkoutPlan { Id = 1, Name = "Morning Cardio", Description = "30-min morning run and stretching", Difficulty = "Beginner", CreatedByUserId = 2 },
            new WorkoutPlan { Id = 2, Name = "Strength Builder", Description = "Full body strength training", Difficulty = "Intermediate", CreatedByUserId = 2 },
            new WorkoutPlan { Id = 3, Name = "HIIT Blast", Description = "High intensity interval training", Difficulty = "Advanced", CreatedByUserId = 2 }
        );

        b.Entity<BodyWeight>().HasData(
            new BodyWeight { Id = 1, UserId = 3, Weight = 72.5f, Date = new DateTime(2025, 5, 1), Note = "Starting weight" },
            new BodyWeight { Id = 2, UserId = 3, Weight = 71.8f, Date = new DateTime(2025, 5, 8) },
            new BodyWeight { Id = 3, UserId = 3, Weight = 71.2f, Date = new DateTime(2025, 5, 15) },
            new BodyWeight { Id = 4, UserId = 3, Weight = 70.5f, Date = new DateTime(2025, 5, 22) },
            new BodyWeight { Id = 5, UserId = 4, Weight = 85f, Date = new DateTime(2025, 5, 1) },
            new BodyWeight { Id = 6, UserId = 4, Weight = 84.2f, Date = new DateTime(2025, 5, 10) }
        );

        b.Entity<Goal>().HasData(
            new Goal { Id = 1, UserId = 3, Title = "Lose 5kg", Type = "Weight", TargetValue = 67.5f, CurrentValue = 70.5f, Deadline = new DateTime(2025, 8, 1), IsCompleted = false, PreferredTrainerGender = "Any" },
            new Goal { Id = 2, UserId = 3, Title = "Run 5km daily", Type = "Workout", TargetValue = 5, CurrentValue = 3, Deadline = new DateTime(2025, 7, 1), IsCompleted = false, PreferredTrainerGender = "Any" },
            new Goal { Id = 3, UserId = 4, Title = "Build muscle mass", Type = "Weight", TargetValue = 90, CurrentValue = 84.2f, Deadline = new DateTime(2025, 9, 1), IsCompleted = false, PreferredTrainerGender = "Any" }
        );

        b.Entity<WorkoutLog>().HasData(
            new WorkoutLog { Id = 1, UserId = 3, WorkoutPlanId = 1, Date = new DateTime(2025, 5, 20), DurationMinutes = 35, CaloriesBurned = 280, Notes = "Felt great!" },
            new WorkoutLog { Id = 2, UserId = 3, WorkoutPlanId = 1, Date = new DateTime(2025, 5, 21), DurationMinutes = 30, CaloriesBurned = 250 },
            new WorkoutLog { Id = 3, UserId = 3, WorkoutPlanId = 2, Date = new DateTime(2025, 5, 22), DurationMinutes = 45, CaloriesBurned = 320 },
            new WorkoutLog { Id = 4, UserId = 4, WorkoutPlanId = 2, Date = new DateTime(2025, 5, 21), DurationMinutes = 60, CaloriesBurned = 400 }
        );

        b.Entity<NutritionLog>().HasData(
            new NutritionLog { Id = 1, UserId = 3, FoodId = 4, Grams = 200, Date = new DateTime(2025, 5, 22), MealType = "Lunch" },
            new NutritionLog { Id = 2, UserId = 3, FoodId = 6, Grams = 150, Date = new DateTime(2025, 5, 22), MealType = "Breakfast" },
            new NutritionLog { Id = 3, UserId = 3, FoodId = 5, Grams = 150, Date = new DateTime(2025, 5, 22), MealType = "Dinner" }
        );

        b.Entity<MoodLog>().HasData(
            new MoodLog { Id = 1, UserId = 3, MoodScore = 4, EnergyScore = 5, Date = new DateTime(2025, 5, 22), Note = "Feeling motivated!" },
            new MoodLog { Id = 2, UserId = 3, MoodScore = 3, EnergyScore = 3, Date = new DateTime(2025, 5, 21) }
        );

        b.Entity<Badge>().HasData(
            new Badge { Id = 1, UserId = 3, Name = "First Workout", Icon = "🏋️", Description = "Completed your first workout", EarnedAt = new DateTime(2025, 5, 20) },
            new Badge { Id = 2, UserId = 3, Name = "3-Day Streak", Icon = "🔥", Description = "Worked out 3 days in a row", EarnedAt = new DateTime(2025, 5, 22) }
        );
    }
}
