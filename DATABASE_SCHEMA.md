# FitTrack Database Schema & Queries

## Database Information

- **Database Type**: SQLite
- **Database File**: `fittrack.db`
- **Connection String**: `Data Source=fittrack.db`
- **ORM**: Entity Framework Core (.NET 10)
- **DbContext**: `AppDbContext` (namespace: `fit.Data`)

---

## Database Tables & Models

### 1. **Roles** Table
**Purpose**: Define user roles in the system

```csharp
public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ICollection<User> Users { get; set; } = [];
}
```

**Seed Data**:
```
Id=1, Name="Admin"
Id=2, Name="Trainer"
Id=3, Name="User"
```

**Relationships**: One-to-Many with Users

---

### 2. **Users** Table
**Purpose**: Store user account information and authentication

```csharp
public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public int RoleId { get; set; }
    public Role Role { get; set; } = null!;
    public int? TrainerId { get; set; }  // Nullable reference to trainer
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Navigation properties
    public ICollection<BodyWeight> BodyWeights { get; set; } = [];
    public ICollection<Goal> Goals { get; set; } = [];
    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = [];
    public ICollection<NutritionLog> NutritionLogs { get; set; } = [];
    public ICollection<MoodLog> MoodLogs { get; set; } = [];
    public ICollection<Badge> Badges { get; set; } = [];
}
```

**Seed Data**:
```
Id=1, Name="Admin User", Email="admin@fittrack.rw", RoleId=1
Id=2, Name="Jean Trainer", Email="trainer@fittrack.rw", RoleId=2
Id=3, Name="Alice Uwase", Email="alice@fittrack.rw", RoleId=3, TrainerId=2
Id=4, Name="Bob Mugisha", Email="bob@fittrack.rw", RoleId=3, TrainerId=2
```

**Relationships**: 
- Many-to-One with Roles
- One-to-Many with BodyWeights, Goals, WorkoutLogs, NutritionLogs, MoodLogs, Badges

---

### 3. **BodyWeights** Table
**Purpose**: Track user weight history

```csharp
public class BodyWeight
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public float Weight { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}
```

**Seed Data** (Sample):
```
Id=1, UserId=3, Weight=72.5, Date=2025-05-01, Note="Starting weight"
Id=2, UserId=3, Weight=71.8, Date=2025-05-08
Id=3, UserId=3, Weight=71.2, Date=2025-05-15
Id=4, UserId=3, Weight=70.5, Date=2025-05-22
Id=5, UserId=4, Weight=85.0, Date=2025-05-01
Id=6, UserId=4, Weight=84.2, Date=2025-05-10
```

**Relationships**: Many-to-One with Users

---

### 4. **Goals** Table
**Purpose**: Store user fitness/health goals

```csharp
public class Goal
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Title { get; set; } = "";
    public string Type { get; set; } = "Weight";  // e.g., "Weight", "Workout"
    public float TargetValue { get; set; }
    public float CurrentValue { get; set; }
    public DateTime Deadline { get; set; }
    public bool IsCompleted { get; set; }
}
```

**Seed Data**:
```
Id=1, UserId=3, Title="Lose 5kg", Type="Weight", TargetValue=67.5, CurrentValue=70.5, Deadline=2025-08-01, IsCompleted=false
Id=2, UserId=3, Title="Run 5km daily", Type="Workout", TargetValue=5, CurrentValue=3, Deadline=2025-07-01, IsCompleted=false
Id=3, UserId=4, Title="Build muscle mass", Type="Weight", TargetValue=90, CurrentValue=84.2, Deadline=2025-09-01, IsCompleted=false
```

**Relationships**: Many-to-One with Users

---

### 5. **Foods** Table
**Purpose**: Food database with nutritional information (local and international)

```csharp
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
```

**Seed Data**:
```
Id=1, Name="Ugali", Category="Grains", CaloriesPer100g=360, ProteinPer100g=3, CarbsPer100g=79, FatPer100g=1, IsLocal=true
Id=2, Name="Isombe (Cassava Leaves)", Category="Vegetables", CaloriesPer100g=45, ProteinPer100g=4, CarbsPer100g=6, FatPer100g=1, IsLocal=true
Id=3, Name="Ibirayi (Potatoes)", Category="Vegetables", CaloriesPer100g=77, ProteinPer100g=2, CarbsPer100g=17, FatPer100g=0.1, IsLocal=true
Id=4, Name="Beans (Ibishyimbo)", Category="Legumes", CaloriesPer100g=347, ProteinPer100g=22, CarbsPer100g=63, FatPer100g=1, IsLocal=true
Id=5, Name="Chicken Breast", Category="Protein", CaloriesPer100g=165, ProteinPer100g=31, CarbsPer100g=0, FatPer100g=3.6, IsLocal=false
Id=6, Name="Banana (Igitoki)", Category="Fruits", CaloriesPer100g=89, ProteinPer100g=1.1, CarbsPer100g=23, FatPer100g=0.3, IsLocal=true
Id=7, Name="Milk (Amata)", Category="Dairy", CaloriesPer100g=61, ProteinPer100g=3.2, CarbsPer100g=4.8, FatPer100g=3.3, IsLocal=true
Id=8, Name="Sweet Potato (Ibijumba)", Category="Vegetables", CaloriesPer100g=86, ProteinPer100g=1.6, CarbsPer100g=20, FatPer100g=0.1, IsLocal=true
```

**Relationships**: One-to-Many with NutritionLogs

---

### 6. **NutritionLogs** Table
**Purpose**: Track food consumption per meal

```csharp
public class NutritionLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int FoodId { get; set; }
    public Food Food { get; set; } = null!;
    public float Grams { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string MealType { get; set; } = "Lunch";  // e.g., "Breakfast", "Lunch", "Dinner"
}
```

**Seed Data** (Sample):
```
Id=1, UserId=3, FoodId=4, Grams=200, Date=2025-05-22, MealType="Lunch"
Id=2, UserId=3, FoodId=6, Grams=150, Date=2025-05-22, MealType="Breakfast"
Id=3, UserId=3, FoodId=5, Grams=150, Date=2025-05-22, MealType="Dinner"
```

**Relationships**: Many-to-One with Users and Foods

---

### 7. **WorkoutPlans** Table
**Purpose**: Define workout plans created by trainers/admins

```csharp
public class WorkoutPlan
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Difficulty { get; set; } = "Beginner";  // "Beginner", "Intermediate", "Advanced"
    public int CreatedByUserId { get; set; }
    public ICollection<WorkoutLog> Logs { get; set; } = [];
}
```

**Seed Data**:
```
Id=1, Name="Morning Cardio", Description="30-min morning run and stretching", Difficulty="Beginner", CreatedByUserId=2
Id=2, Name="Strength Builder", Description="Full body strength training", Difficulty="Intermediate", CreatedByUserId=2
Id=3, Name="HIIT Blast", Description="High intensity interval training", Difficulty="Advanced", CreatedByUserId=2
```

**Relationships**: One-to-Many with WorkoutLogs

---

### 8. **WorkoutLogs** Table
**Purpose**: Track completed workouts

```csharp
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
```

**Seed Data** (Sample):
```
Id=1, UserId=3, WorkoutPlanId=1, Date=2025-05-20, DurationMinutes=35, CaloriesBurned=280, Notes="Felt great!"
Id=2, UserId=3, WorkoutPlanId=1, Date=2025-05-21, DurationMinutes=30, CaloriesBurned=250
Id=3, UserId=3, WorkoutPlanId=2, Date=2025-05-22, DurationMinutes=45, CaloriesBurned=320
Id=4, UserId=4, WorkoutPlanId=2, Date=2025-05-21, DurationMinutes=60, CaloriesBurned=400
```

**Relationships**: Many-to-One with Users and WorkoutPlans

---

### 9. **MoodLogs** Table
**Purpose**: Track user mood and energy levels

```csharp
public class MoodLog
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public int MoodScore { get; set; }  // Scale: 1-5
    public int EnergyScore { get; set; }  // Scale: 1-5
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }
}
```

**Seed Data** (Sample):
```
Id=1, UserId=3, MoodScore=4, EnergyScore=5, Date=2025-05-22, Note="Feeling motivated!"
Id=2, UserId=3, MoodScore=3, EnergyScore=3, Date=2025-05-21
```

**Relationships**: Many-to-One with Users

---

### 10. **Badges** Table
**Purpose**: Track achievements earned by users

```csharp
public class Badge
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Name { get; set; } = "";
    public string Icon { get; set; } = "🏅";  // Emoji icon
    public string Description { get; set; } = "";
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;
}
```

**Seed Data** (Sample):
```
Id=1, UserId=3, Name="First Workout", Icon="🏋️", Description="Completed your first workout", EarnedAt=2025-05-20
Id=2, UserId=3, Name="3-Day Streak", Icon="🔥", Description="Worked out 3 days in a row", EarnedAt=2025-05-22
```

**Relationships**: Many-to-One with Users

---

## Database Queries by Page

### **Pages/Index.cshtml.cs** (Landing Page)

#### Query 1: Get Top 3 Trainers
```csharp
TopTrainers = await _db.Users
    .Where(u => u.RoleId == 2)
    .Take(3)
    .ToListAsync();
```
**Purpose**: Display featured trainers on landing page
**Returns**: List of users with role "Trainer"

---

### **Pages/Dashboard/Index.cshtml.cs** (Dashboard)

#### Query 1: Get Current User (Users Only)
```csharp
CurrentUser = await db.Users
    .Include(u => u.Role)
    .FirstOrDefaultAsync(u => u.Id == uid);
```
**Purpose**: Load logged-in user details
**Returns**: Single user with role information

#### Query 2: Get Weight History (Users Only)
```csharp
WeightHistory = await db.BodyWeights
    .Where(b => b.UserId == uid)
    .OrderBy(b => b.Date)
    .Take(10)
    .ToListAsync();
```
**Purpose**: Get last 10 weight entries for chart
**Returns**: List of body weight records ordered by date

#### Query 3: Get Recent Workouts (Users Only)
```csharp
RecentWorkouts = await db.WorkoutLogs
    .Include(w => w.WorkoutPlan)
    .Where(w => w.UserId == uid)
    .OrderByDescending(w => w.Date)
    .Take(5)
    .ToListAsync();
```
**Purpose**: Display 5 most recent completed workouts
**Returns**: List of workout logs with associated plans

#### Query 4: Get Today's Nutrition Logs (Users Only)
```csharp
var today = DateTime.UtcNow.Date;
TodayNutrition = await db.NutritionLogs
    .Include(n => n.Food)
    .Where(n => n.UserId == uid && n.Date.Date == today)
    .ToListAsync();
```
**Purpose**: Get all food entries for current day
**Returns**: List of nutrition logs with food details

#### Query 5: Get User Goals (Users Only)
```csharp
Goals = await db.Goals
    .Where(g => g.UserId == uid)
    .ToListAsync();
```
**Purpose**: Load all active and completed goals
**Returns**: List of goals for the user

#### Query 6: Get User Badges (Users Only)
```csharp
Badges = await db.Badges
    .Where(b => b.UserId == uid)
    .OrderByDescending(b => b.EarnedAt)
    .Take(6)
    .ToListAsync();
```
**Purpose**: Display 6 most recent earned badges
**Returns**: List of badges ordered by earned date

#### Query 7: Get Latest Mood Entry (Users Only)
```csharp
LatestMood = await db.MoodLogs
    .Where(m => m.UserId == uid)
    .OrderByDescending(m => m.Date)
    .FirstOrDefaultAsync();
```
**Purpose**: Get most recent mood/energy log
**Returns**: Single mood log or null

#### Query 8: Calculate Workout Streak (Users Only)
```csharp
var logDates = await db.WorkoutLogs
    .Where(w => w.UserId == uid)
    .Select(w => w.Date.Date)
    .Distinct()
    .OrderByDescending(d => d)
    .ToListAsync();
```
**Purpose**: Get distinct workout dates for streak calculation
**Returns**: List of dates with workouts

#### Query 9: Get Admin Stats
```csharp
TotalUsers = await db.Users.CountAsync();
TotalWorkoutLogs = await db.WorkoutLogs.CountAsync();
TotalFoods = await db.Foods.CountAsync();
var recentUsers = await db.Users
    .Include(u => u.Role)
    .OrderByDescending(u => u.CreatedAt)
    .Take(5)
    .ToListAsync();
```
**Purpose**: Display admin dashboard statistics
**Returns**: Count of users, workout logs, foods; recent user list

#### Query 10: Get Trainer's Clients & Stats
```csharp
Clients = await db.Users
    .Include(u => u.Role)
    .Where(u => u.TrainerId == uid)
    .ToListAsync();

RecentWorkouts = await db.WorkoutLogs
    .Include(w => w.WorkoutPlan)
    .Include(w => w.User)
    .Where(w => Clients.Select(c => c.Id).Contains(w.UserId))
    .OrderByDescending(w => w.Date)
    .Take(10)
    .ToListAsync();
```
**Purpose**: Get trainer's client list and their recent workouts
**Returns**: List of users and their workout logs

---

### **Pages/Admin/Users.cshtml.cs** (User Management)

#### Query 1: Get All Users
```csharp
Users = await db.Users
    .Include(u => u.Role)
    .OrderBy(u => u.Name)
    .ToListAsync();
```
**Purpose**: List all users for admin management
**Returns**: Sorted list of all users with roles

#### Query 2: Get All Roles
```csharp
Roles = await db.Roles.ToListAsync();
```
**Purpose**: Populate role dropdown for user editing
**Returns**: List of all available roles

#### Query 3: Find User by ID
```csharp
var existing = await db.Users.FindAsync(EditUser.Id);
```
**Purpose**: Retrieve user for editing/deletion
**Returns**: Single user or null

---

### **Pages/Admin/WorkoutPlans.cshtml.cs** (Workout Plan Management)

#### Query 1: Get All Workout Plans
```csharp
Plans = await db.WorkoutPlans
    .OrderBy(p => p.Name)
    .ToListAsync();
```
**Purpose**: List all workout plans
**Returns**: Sorted list of workout plans

#### Query 2: Get All Trainers
```csharp
Trainers = await db.Users
    .Where(u => u.RoleId == 2)
    .ToListAsync();
```
**Purpose**: Populate trainer dropdown
**Returns**: List of users with role "Trainer"

#### Query 3: Find Plan by ID
```csharp
var plan = await db.WorkoutPlans.FindAsync(id);
```
**Purpose**: Get specific plan for editing/deletion
**Returns**: Single workout plan or null

---

### **Pages/Admin/Foods.cshtml.cs** (Food Database Management)

#### Query 1: Get All Foods
```csharp
Foods = await db.Foods
    .OrderBy(f => f.Category)
    .ThenBy(f => f.Name)
    .ToListAsync();
```
**Purpose**: List all foods sorted by category then name
**Returns**: Sorted list of all foods

#### Query 2: Find Food by ID
```csharp
var food = await db.Foods.FindAsync(id);
```
**Purpose**: Get specific food for editing/deletion
**Returns**: Single food item or null

---

### **Pages/User/BodyWeight.cshtml.cs** (Weight Tracking)

#### Query 1: Get Weight History with Date Range Filter
```csharp
var q = db.BodyWeights.Where(b => b.UserId == uid);
if (DateTime.TryParse(dateFrom, out var df)) q = q.Where(b => b.Date >= df);
if (DateTime.TryParse(dateTo, out var dt)) q = q.Where(b => b.Date <= dt);
Entries = await q.OrderByDescending(b => b.Date).ToListAsync();
```
**Purpose**: Get user's weight entries with optional date filtering
**Returns**: Filtered and sorted list of weight entries

#### Query 2: Find Weight Entry by ID
```csharp
var existing = await db.BodyWeights.FindAsync(Entry.Id);
```
**Purpose**: Get specific entry for editing
**Returns**: Single weight entry or null

---

### **Pages/User/Nutrition.cshtml.cs** (Nutrition Tracking)

#### Query 1: Get All Foods
```csharp
Foods = await db.Foods
    .OrderBy(f => f.Name)
    .ToListAsync();
```
**Purpose**: Populate food dropdown
**Returns**: Sorted list of foods

#### Query 2: Get Nutrition Logs for Specific Date
```csharp
var filterDate = DateTime.TryParse(DateFilter, out var d) ? d.Date : DateTime.UtcNow.Date;
Logs = await db.NutritionLogs
    .Include(n => n.Food)
    .Where(n => n.UserId == uid && n.Date.Date == filterDate)
    .OrderBy(n => n.MealType)
    .ToListAsync();
```
**Purpose**: Get all meals logged for a specific date
**Returns**: Nutrition logs with food details, sorted by meal type

#### Query 3: Find Nutrition Log by ID
```csharp
var existing = await db.NutritionLogs.FindAsync(Entry.Id);
```
**Purpose**: Get specific nutrition entry for editing
**Returns**: Single nutrition log or null

---

### **Pages/User/Workouts.cshtml.cs** (Workout Tracking)

#### Query 1: Get All Workout Plans
```csharp
Plans = await db.WorkoutPlans
    .OrderBy(p => p.Name)
    .ToListAsync();
```
**Purpose**: Populate workout plan dropdown
**Returns**: Sorted list of workout plans

#### Query 2: Get Workout Logs with Date Range Filter
```csharp
var q = db.WorkoutLogs
    .Include(w => w.WorkoutPlan)
    .Where(w => w.UserId == uid);
if (DateTime.TryParse(dateFrom, out var df)) q = q.Where(w => w.Date >= df);
if (DateTime.TryParse(dateTo, out var dt)) q = q.Where(w => w.Date <= dt);
Logs = await q.OrderByDescending(w => w.Date).ToListAsync();
```
**Purpose**: Get user's completed workouts with optional date range
**Returns**: Filtered and sorted list of workout logs

#### Query 3: Find Workout Log by ID
```csharp
var existing = await db.WorkoutLogs.FindAsync(Entry.Id);
```
**Purpose**: Get specific workout for editing
**Returns**: Single workout log or null

---

### **Pages/User/Goals.cshtml.cs** (Goal Tracking)

#### Query 1: Get User's Goals
```csharp
Goals = await db.Goals
    .Where(g => g.UserId == SessionUserId!.Value)
    .OrderBy(g => g.IsCompleted)
    .ThenBy(g => g.Deadline)
    .ToListAsync();
```
**Purpose**: Get all user goals, active goals first, sorted by deadline
**Returns**: Sorted list of goals

#### Query 2: Find Goal by ID
```csharp
var existing = await db.Goals.FindAsync(Entry.Id);
```
**Purpose**: Get specific goal for editing
**Returns**: Single goal or null

---

### **Pages/User/Mood.cshtml.cs** (Mood Tracking)

#### Query 1: Get Recent Mood Logs
```csharp
Logs = await db.MoodLogs
    .Where(m => m.UserId == uid)
    .OrderByDescending(m => m.Date)
    .Take(14)
    .ToListAsync();
```
**Purpose**: Get last 14 mood entries for chart
**Returns**: Last 14 mood logs ordered by date

#### Query 2: Find Mood Log by ID
```csharp
var existing = await db.MoodLogs.FindAsync(Entry.Id);
```
**Purpose**: Get specific mood entry for editing
**Returns**: Single mood log or null

---

### **Pages/Trainer/Clients.cshtml.cs** (Trainer's Client Management)

#### Query 1: Get Trainer's Clients
```csharp
Clients = await db.Users
    .Where(u => u.TrainerId == uid)
    .ToListAsync();
```
**Purpose**: Get list of users assigned to trainer
**Returns**: List of client users

#### Query 2: Get Latest Weight for Each Client
```csharp
foreach (var c in Clients)
{
    LatestWeights[c.Id] = await db.BodyWeights
        .Where(b => b.UserId == c.Id)
        .OrderByDescending(b => b.Date)
        .FirstOrDefaultAsync();
}
```
**Purpose**: Get most recent weight entry for each client
**Returns**: Dictionary of client ID to latest weight

#### Query 3: Get Workout Count for Each Client
```csharp
foreach (var c in Clients)
{
    WorkoutCounts[c.Id] = await db.WorkoutLogs
        .CountAsync(w => w.UserId == c.Id);
}
```
**Purpose**: Count total workouts logged by each client
**Returns**: Dictionary of client ID to workout count

---

### **Pages/Admin/Reports.cshtml.cs** (Analytics & Reports)

#### Query 1: Get Total Counts
```csharp
TotalUsers = await db.Users.CountAsync();
TotalWorkouts = await db.WorkoutLogs.CountAsync();
TotalNutritionLogs = await db.NutritionLogs.CountAsync();
```
**Purpose**: Get system-wide statistics
**Returns**: Total counts for each entity

#### Query 2: Group Users by Role
```csharp
var roleGroups = await db.Users
    .Include(u => u.Role)
    .GroupBy(u => u.Role.Name)
    .Select(g => new { Role = g.Key, Count = g.Count() })
    .ToListAsync();
```
**Purpose**: Create data for pie chart of users by role
**Returns**: List of role names with user counts

#### Query 3: Group Workouts by Month
```csharp
var monthGroups = await db.WorkoutLogs
    .GroupBy(w => new { w.Date.Year, w.Date.Month })
    .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
    .OrderBy(g => g.Year)
    .ThenBy(g => g.Month)
    .Take(6)
    .ToListAsync();
```
**Purpose**: Create data for bar chart of workouts by month (last 6 months)
**Returns**: List of months with workout counts

#### Query 4: Calculate Average Daily Calories
```csharp
var nutritionLogs = await db.NutritionLogs
    .Include(n => n.Food)
    .ToListAsync();

if (nutritionLogs.Any())
{
    var totalCals = nutritionLogs.Sum(n => n.Food.CaloriesPer100g * n.Grams / 100);
    var days = nutritionLogs.Select(n => n.Date.Date).Distinct().Count();
    AvgCaloriesPerDay = days > 0 ? totalCals / days : 0;
}
```
**Purpose**: Calculate average daily calorie intake across all users
**Returns**: Float value for average calories per day

---

## Key Database Relationships

```
Role (1) -----> (Many) User
         RoleId

User (1) -----> (Many) BodyWeight
     Id          UserId

User (1) -----> (Many) Goal
     Id          UserId

User (1) -----> (Many) NutritionLog
     Id          UserId

Food (1) -----> (Many) NutritionLog
   Id            FoodId

User (1) -----> (Many) WorkoutLog
     Id          UserId

WorkoutPlan (1) -----> (Many) WorkoutLog
         Id              WorkoutPlanId

User (1) -----> (Many) MoodLog
     Id          UserId

User (1) -----> (Many) Badge
     Id          UserId
```

---

## Seeding Data

All seed data is configured in the `OnModelCreating` method of `AppDbContext.cs`:

- **3 Roles**: Admin, Trainer, User
- **4 Sample Users**: 1 Admin, 1 Trainer, 2 Regular Users
- **8 Foods**: Local Rwandan foods and common proteins/vegetables
- **3 Workout Plans**: Morning Cardio, Strength Builder, HIIT Blast
- **6 Body Weight entries**: Sample weight progression
- **3 Goals**: Fitness goals for users
- **4 Workout Logs**: Sample completed workouts
- **3 Nutrition Logs**: Sample meal logs
- **2 Mood Logs**: Sample mood entries
- **2 Badges**: Earned achievements

---

## Common Query Patterns

### Authentication & Authorization
- Check session user ID exists
- Validate user role (Admin, Trainer, User)
- Verify ownership before edit/delete (UserId match)

### Data Filtering
- Filter by UserId for user-specific records
- Filter by date range (dateFrom, dateTo)
- Filter by date.Date for daily queries
- Filter by RoleId for role-based listings

### Data Aggregation
- Sum calories from nutrition logs
- Count workouts for streak calculation
- Group by role, month, category
- Calculate averages (calories per day)

### Data Ordering
- OrderBy date for chronological display
- OrderByDescending for most recent first
- ThenBy for secondary sorting

### Relationships
- Include() for eager loading related data
- Select() for projection to specific fields
- Distinct() to remove duplicates

---

## Notes

- All dates stored in UTC (DateTime.UtcNow)
- Password hashing uses BCrypt.Net
- SQLite limitations: No spatial data, limited JSON support
- Migration file: `20260417151638_InitialCreate.cs`
- Database created on first app run if not exists
