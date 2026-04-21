-- ========================================
-- FITTRACK DATABASE - SQL QUERIES FOR SSMS
-- ========================================

-- ========================================
-- 1. CREATE TABLES
-- ========================================

CREATE TABLE Roles (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PasswordHash NVARCHAR(MAX) NOT NULL,
    RoleId INT NOT NULL,
    TrainerId INT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id),
    FOREIGN KEY (TrainerId) REFERENCES Users(Id)
);

CREATE TABLE BodyWeights (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Weight FLOAT NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Note NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Goals (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Title NVARCHAR(255) NOT NULL,
    [Type] NVARCHAR(100) NOT NULL DEFAULT 'Weight',
    TargetValue FLOAT NOT NULL,
    CurrentValue FLOAT NOT NULL,
    Deadline DATETIME NOT NULL,
    IsCompleted BIT NOT NULL DEFAULT 0,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Foods (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Category NVARCHAR(100) NOT NULL,
    CaloriesPer100g FLOAT NOT NULL,
    ProteinPer100g FLOAT NOT NULL,
    CarbsPer100g FLOAT NOT NULL,
    FatPer100g FLOAT NOT NULL,
    IsLocal BIT NOT NULL DEFAULT 0
);

CREATE TABLE NutritionLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    FoodId INT NOT NULL,
    Grams FLOAT NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    MealType NVARCHAR(100) NOT NULL DEFAULT 'Lunch',
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (FoodId) REFERENCES Foods(Id)
);

CREATE TABLE WorkoutPlans (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(255) NOT NULL,
    Description NVARCHAR(MAX) NOT NULL,
    Difficulty NVARCHAR(100) NOT NULL DEFAULT 'Beginner',
    CreatedByUserId INT NOT NULL,
    FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);

CREATE TABLE WorkoutLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    WorkoutPlanId INT NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    DurationMinutes INT NOT NULL,
    CaloriesBurned INT NOT NULL,
    Notes NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (WorkoutPlanId) REFERENCES WorkoutPlans(Id)
);

CREATE TABLE MoodLogs (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    MoodScore INT NOT NULL,
    EnergyScore INT NOT NULL,
    [Date] DATETIME NOT NULL DEFAULT GETUTCDATE(),
    Note NVARCHAR(MAX) NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

CREATE TABLE Badges (
    Id INT PRIMARY KEY IDENTITY(1,1),
    UserId INT NOT NULL,
    Name NVARCHAR(255) NOT NULL,
    Icon NVARCHAR(255) NOT NULL DEFAULT '🏅',
    Description NVARCHAR(MAX) NOT NULL,
    EarnedAt DATETIME NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- ========================================
-- 2. SEED DATA - ROLES
-- ========================================

INSERT INTO Roles (Name) VALUES ('Admin');
INSERT INTO Roles (Name) VALUES ('Trainer');
INSERT INTO Roles (Name) VALUES ('User');

-- ========================================
-- 3. SEED DATA - USERS
-- ========================================

INSERT INTO Users (Name, Email, PasswordHash, RoleId, CreatedAt) 
VALUES ('Admin User', 'admin@fittrack.rw', '$2a$11$qoF5S9rAPVvLZAhR4OZFM.DvBVfxvX8BPW.3LzKjDYJKBWu1A6Pxm', 1, '2025-01-01');

INSERT INTO Users (Name, Email, PasswordHash, RoleId, CreatedAt) 
VALUES ('Jean Trainer', 'trainer@fittrack.rw', '$2a$11$qoF5S9rAPVvLZAhR4OZFM.DvBVfxvX8BPW.3LzKjDYJKBWu1A6Pxm', 2, '2025-01-01');

INSERT INTO Users (Name, Email, PasswordHash, RoleId, TrainerId, CreatedAt) 
VALUES ('Alice Uwase', 'alice@fittrack.rw', '$2a$11$qoF5S9rAPVvLZAhR4OZFM.DvBVfxvX8BPW.3LzKjDYJKBWu1A6Pxm', 3, 2, '2025-01-01');

INSERT INTO Users (Name, Email, PasswordHash, RoleId, TrainerId, CreatedAt) 
VALUES ('Bob Mugisha', 'bob@fittrack.rw', '$2a$11$qoF5S9rAPVvLZAhR4OZFM.DvBVfxvX8BPW.3LzKjDYJKBWu1A6Pxm', 3, 2, '2025-01-01');

-- ========================================
-- 4. SEED DATA - FOODS
-- ========================================

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Ugali', 'Grains', 360, 3, 79, 1, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Isombe (Cassava Leaves)', 'Vegetables', 45, 4, 6, 1, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Ibirayi (Potatoes)', 'Vegetables', 77, 2, 17, 0.1, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Beans (Ibishyimbo)', 'Legumes', 347, 22, 63, 1, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Chicken Breast', 'Protein', 165, 31, 0, 3.6, 0);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Banana (Igitoki)', 'Fruits', 89, 1.1, 23, 0.3, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Milk (Amata)', 'Dairy', 61, 3.2, 4.8, 3.3, 1);

INSERT INTO Foods (Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal) 
VALUES ('Sweet Potato (Ibijumba)', 'Vegetables', 86, 1.6, 20, 0.1, 1);

-- ========================================
-- 5. SEED DATA - WORKOUT PLANS
-- ========================================

INSERT INTO WorkoutPlans (Name, Description, Difficulty, CreatedByUserId) 
VALUES ('Morning Cardio', '30-min morning run and stretching', 'Beginner', 2);

INSERT INTO WorkoutPlans (Name, Description, Difficulty, CreatedByUserId) 
VALUES ('Strength Builder', 'Full body strength training', 'Intermediate', 2);

INSERT INTO WorkoutPlans (Name, Description, Difficulty, CreatedByUserId) 
VALUES ('HIIT Blast', 'High intensity interval training', 'Advanced', 2);

-- ========================================
-- 6. SEED DATA - BODY WEIGHTS
-- ========================================

INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (3, 72.5, '2025-05-01', 'Starting weight');
INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (3, 71.8, '2025-05-08', NULL);
INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (3, 71.2, '2025-05-15', NULL);
INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (3, 70.5, '2025-05-22', NULL);
INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (4, 85.0, '2025-05-01', NULL);
INSERT INTO BodyWeights (UserId, Weight, [Date], Note) VALUES (4, 84.2, '2025-05-10', NULL);

-- ========================================
-- 7. SEED DATA - GOALS
-- ========================================

INSERT INTO Goals (UserId, Title, [Type], TargetValue, CurrentValue, Deadline, IsCompleted) 
VALUES (3, 'Lose 5kg', 'Weight', 67.5, 70.5, '2025-08-01', 0);

INSERT INTO Goals (UserId, Title, [Type], TargetValue, CurrentValue, Deadline, IsCompleted) 
VALUES (3, 'Run 5km daily', 'Workout', 5, 3, '2025-07-01', 0);

INSERT INTO Goals (UserId, Title, [Type], TargetValue, CurrentValue, Deadline, IsCompleted) 
VALUES (4, 'Build muscle mass', 'Weight', 90, 84.2, '2025-09-01', 0);

-- ========================================
-- 8. SEED DATA - WORKOUT LOGS
-- ========================================

INSERT INTO WorkoutLogs (UserId, WorkoutPlanId, [Date], DurationMinutes, CaloriesBurned, Notes) 
VALUES (3, 1, '2025-05-20', 35, 280, 'Felt great!');

INSERT INTO WorkoutLogs (UserId, WorkoutPlanId, [Date], DurationMinutes, CaloriesBurned, Notes) 
VALUES (3, 1, '2025-05-21', 30, 250, NULL);

INSERT INTO WorkoutLogs (UserId, WorkoutPlanId, [Date], DurationMinutes, CaloriesBurned, Notes) 
VALUES (3, 2, '2025-05-22', 45, 320, NULL);

INSERT INTO WorkoutLogs (UserId, WorkoutPlanId, [Date], DurationMinutes, CaloriesBurned, Notes) 
VALUES (4, 2, '2025-05-21', 60, 400, NULL);

-- ========================================
-- 9. SEED DATA - NUTRITION LOGS
-- ========================================

INSERT INTO NutritionLogs (UserId, FoodId, Grams, [Date], MealType) VALUES (3, 4, 200, '2025-05-22', 'Lunch');
INSERT INTO NutritionLogs (UserId, FoodId, Grams, [Date], MealType) VALUES (3, 6, 150, '2025-05-22', 'Breakfast');
INSERT INTO NutritionLogs (UserId, FoodId, Grams, [Date], MealType) VALUES (3, 5, 150, '2025-05-22', 'Dinner');

-- ========================================
-- 10. SEED DATA - MOOD LOGS
-- ========================================

INSERT INTO MoodLogs (UserId, MoodScore, EnergyScore, [Date], Note) VALUES (3, 4, 5, '2025-05-22', 'Feeling motivated!');
INSERT INTO MoodLogs (UserId, MoodScore, EnergyScore, [Date], Note) VALUES (3, 3, 3, '2025-05-21', NULL);

-- ========================================
-- 11. SEED DATA - BADGES
-- ========================================

INSERT INTO Badges (UserId, Name, Icon, Description, EarnedAt) 
VALUES (3, 'First Workout', '🏋️', 'Completed your first workout', '2025-05-20');

INSERT INTO Badges (UserId, Name, Icon, Description, EarnedAt) 
VALUES (3, '3-Day Streak', '🔥', 'Worked out 3 days in a row', '2025-05-22');

-- ========================================
-- 12. COMMON SELECT QUERIES
-- ========================================

-- Get all users with their roles
SELECT u.Id, u.Name, u.Email, r.Name AS Role, u.CreatedAt 
FROM Users u
INNER JOIN Roles r ON u.RoleId = r.Id
ORDER BY u.Name;

-- Get top 3 trainers
SELECT Id, Name, Email 
FROM Users 
WHERE RoleId = 2
ORDER BY CreatedAt DESC
LIMIT 3;

-- Get user's weight history
SELECT bw.Id, bw.Weight, bw.[Date], bw.Note
FROM BodyWeights bw
WHERE bw.UserId = 3
ORDER BY bw.[Date] DESC;

-- Get user's goals
SELECT Id, Title, [Type], TargetValue, CurrentValue, Deadline, IsCompleted
FROM Goals
WHERE UserId = 3
ORDER BY IsCompleted, Deadline;

-- Get today's nutrition logs
SELECT nl.Id, f.Name AS Food, nl.Grams, f.CaloriesPer100g, nl.MealType
FROM NutritionLogs nl
INNER JOIN Foods f ON nl.FoodId = f.Id
WHERE nl.UserId = 3 AND CAST(nl.[Date] AS DATE) = CAST(GETUTCDATE() AS DATE)
ORDER BY nl.MealType;

-- Get recent workouts
SELECT wl.Id, wp.Name, wl.DurationMinutes, wl.CaloriesBurned, wl.[Date], wl.Notes
FROM WorkoutLogs wl
INNER JOIN WorkoutPlans wp ON wl.WorkoutPlanId = wp.Id
WHERE wl.UserId = 3
ORDER BY wl.[Date] DESC;

-- Get user's badges
SELECT Id, Name, Icon, Description, EarnedAt
FROM Badges
WHERE UserId = 3
ORDER BY EarnedAt DESC;

-- Get trainer's clients
SELECT Id, Name, Email, CreatedAt
FROM Users
WHERE TrainerId = 2;

-- Get workout streak count
SELECT COUNT(DISTINCT CAST([Date] AS DATE)) AS WorkoutDays
FROM WorkoutLogs
WHERE UserId = 3;

-- Get all foods by category
SELECT Name, Category, CaloriesPer100g, ProteinPer100g, CarbsPer100g, FatPer100g, IsLocal
FROM Foods
ORDER BY Category, Name;

-- Get admin stats
SELECT 
    (SELECT COUNT(*) FROM Users) AS TotalUsers,
    (SELECT COUNT(*) FROM WorkoutLogs) AS TotalWorkouts,
    (SELECT COUNT(*) FROM Foods) AS TotalFoods,
    (SELECT COUNT(*) FROM NutritionLogs) AS TotalNutritionLogs;

-- Get users by role
SELECT r.Name AS Role, COUNT(u.Id) AS UserCount
FROM Users u
INNER JOIN Roles r ON u.RoleId = r.Id
GROUP BY r.Name;

-- Get workouts by month (last 6 months)
SELECT 
    YEAR(wl.[Date]) AS Year,
    MONTH(wl.[Date]) AS Month,
    FORMAT(wl.[Date], 'MMM yyyy') AS MonthYear,
    COUNT(*) AS WorkoutCount
FROM WorkoutLogs wl
GROUP BY YEAR(wl.[Date]), MONTH(wl.[Date])
ORDER BY Year DESC, Month DESC
OFFSET 0 ROWS FETCH NEXT 6 ROWS ONLY;

-- Get average daily calories
SELECT 
    SUM(f.CaloriesPer100g * nl.Grams / 100) / 
    COUNT(DISTINCT CAST(nl.[Date] AS DATE)) AS AvgCaloriesPerDay
FROM NutritionLogs nl
INNER JOIN Foods f ON nl.FoodId = f.Id;

-- Get latest mood entry
SELECT TOP 1 Id, MoodScore, EnergyScore, [Date], Note
FROM MoodLogs
WHERE UserId = 3
ORDER BY [Date] DESC;

-- Get trainer's client latest weight
SELECT u.Id, u.Name,
       (SELECT TOP 1 Weight FROM BodyWeights WHERE UserId = u.Id ORDER BY [Date] DESC) AS LatestWeight
FROM Users u
WHERE u.TrainerId = 2;

-- Get client workout count
SELECT u.Id, u.Name, COUNT(wl.Id) AS TotalWorkouts
FROM Users u
LEFT JOIN WorkoutLogs wl ON u.Id = wl.UserId
WHERE u.TrainerId = 2
GROUP BY u.Id, u.Name;

-- ========================================
-- 13. UPDATE & DELETE QUERIES
-- ========================================

-- Update user
UPDATE Users
SET Name = 'Updated Name', Email = 'newemail@fittrack.rw'
WHERE Id = 3;

-- Delete weight entry
DELETE FROM BodyWeights
WHERE Id = 1 AND UserId = 3;

-- Delete goal
DELETE FROM Goals
WHERE Id = 1 AND UserId = 3;

-- Delete nutrition log
DELETE FROM NutritionLogs
WHERE Id = 1 AND UserId = 3;

-- Delete workout log
DELETE FROM WorkoutLogs
WHERE Id = 1 AND UserId = 3;

-- Mark goal as completed
UPDATE Goals
SET IsCompleted = 1
WHERE Id = 1 AND UserId = 3;

-- ========================================
-- 14. INDEXES FOR PERFORMANCE
-- ========================================

CREATE INDEX idx_Users_RoleId ON Users(RoleId);
CREATE INDEX idx_Users_TrainerId ON Users(TrainerId);
CREATE INDEX idx_BodyWeights_UserId_Date ON BodyWeights(UserId, [Date] DESC);
CREATE INDEX idx_Goals_UserId ON Goals(UserId);
CREATE INDEX idx_NutritionLogs_UserId_Date ON NutritionLogs(UserId, [Date] DESC);
CREATE INDEX idx_WorkoutLogs_UserId_Date ON WorkoutLogs(UserId, [Date] DESC);
CREATE INDEX idx_MoodLogs_UserId_Date ON MoodLogs(UserId, [Date] DESC);
CREATE INDEX idx_Badges_UserId ON Badges(UserId);
