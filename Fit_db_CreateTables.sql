-- ========================================
-- FitTrack - Create tables for SQL Server
-- Target DB: Fit_db (SSMS)
-- ========================================

USE [Fit_db];
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- Drop order (safe re-run)
IF OBJECT_ID(N'dbo.WorkoutLogs', N'U') IS NOT NULL DROP TABLE dbo.WorkoutLogs;
IF OBJECT_ID(N'dbo.NutritionLogs', N'U') IS NOT NULL DROP TABLE dbo.NutritionLogs;
IF OBJECT_ID(N'dbo.MoodLogs', N'U') IS NOT NULL DROP TABLE dbo.MoodLogs;
IF OBJECT_ID(N'dbo.Goals', N'U') IS NOT NULL DROP TABLE dbo.Goals;
IF OBJECT_ID(N'dbo.BodyWeights', N'U') IS NOT NULL DROP TABLE dbo.BodyWeights;
IF OBJECT_ID(N'dbo.Badges', N'U') IS NOT NULL DROP TABLE dbo.Badges;
IF OBJECT_ID(N'dbo.WorkoutPlans', N'U') IS NOT NULL DROP TABLE dbo.WorkoutPlans;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL DROP TABLE dbo.Users;
IF OBJECT_ID(N'dbo.Foods', N'U') IS NOT NULL DROP TABLE dbo.Foods;
IF OBJECT_ID(N'dbo.Roles', N'U') IS NOT NULL DROP TABLE dbo.Roles;
IF OBJECT_ID(N'dbo.Locations', N'U') IS NOT NULL DROP TABLE dbo.Locations;
IF OBJECT_ID(N'dbo.TwoFactorTokens', N'U') IS NOT NULL DROP TABLE dbo.TwoFactorTokens;
GO

-- ========================================
-- Core tables
-- ========================================

CREATE TABLE dbo.Roles (
    Id   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Roles PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL
);

CREATE TABLE dbo.Locations (
    Id         INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Locations PRIMARY KEY,
    Province   NVARCHAR(200) NOT NULL,
    District   NVARCHAR(200) NOT NULL,
    Sector     NVARCHAR(200) NOT NULL,
    Cellule    NVARCHAR(200) NOT NULL,
    Village    NVARCHAR(200) NOT NULL,
    CONSTRAINT UQ_Locations_AdminPath UNIQUE (Province, District, Sector, Cellule, Village)
);
CREATE INDEX IX_Locations_District ON dbo.Locations (Province, District);

CREATE TABLE dbo.Users (
    Id            INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
    Name          NVARCHAR(255) NOT NULL,
    Email         NVARCHAR(255) NOT NULL,
    PasswordHash  NVARCHAR(MAX) NOT NULL,
    RoleId        INT NOT NULL,
    TrainerId     INT NULL,
    LocationId    INT NULL,
    CreatedAt     DATETIME2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Users_Roles_RoleId FOREIGN KEY (RoleId)
        REFERENCES dbo.Roles (Id) ON DELETE CASCADE,
    CONSTRAINT FK_Users_Users_TrainerId FOREIGN KEY (TrainerId)
        REFERENCES dbo.Users (Id) ON DELETE NO ACTION,
    CONSTRAINT FK_Users_Locations_LocationId FOREIGN KEY (LocationId)
        REFERENCES dbo.Locations (Id) ON DELETE NO ACTION
);

-- Recommended constraint for logins
CREATE UNIQUE INDEX UX_Users_Email ON dbo.Users (Email);
CREATE INDEX IX_Users_RoleId ON dbo.Users (RoleId);
CREATE INDEX IX_Users_TrainerId ON dbo.Users (TrainerId);
CREATE INDEX IX_Users_LocationId ON dbo.Users (LocationId);

CREATE TABLE dbo.BodyWeights (
    Id      INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_BodyWeights PRIMARY KEY,
    UserId  INT NOT NULL,
    Weight  REAL NOT NULL,
    [Date]  DATETIME2 NOT NULL CONSTRAINT DF_BodyWeights_Date DEFAULT (SYSUTCDATETIME()),
    Note    NVARCHAR(MAX) NULL,
    CONSTRAINT FK_BodyWeights_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE
);
CREATE INDEX IX_BodyWeights_UserId ON dbo.BodyWeights (UserId);

CREATE TABLE dbo.Goals (
    Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Goals PRIMARY KEY,
    UserId       INT NOT NULL,
    Title        NVARCHAR(255) NOT NULL,
    [Type]       NVARCHAR(100) NOT NULL CONSTRAINT DF_Goals_Type DEFAULT (N'Weight'),
    TargetValue  REAL NOT NULL,
    CurrentValue REAL NOT NULL,
    Deadline     DATETIME2 NOT NULL,
    IsCompleted  BIT NOT NULL CONSTRAINT DF_Goals_IsCompleted DEFAULT (0),
    CONSTRAINT FK_Goals_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE
);
CREATE INDEX IX_Goals_UserId ON dbo.Goals (UserId);

CREATE TABLE dbo.Foods (
    Id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Foods PRIMARY KEY,
    Name            NVARCHAR(255) NOT NULL,
    Category        NVARCHAR(100) NOT NULL,
    CaloriesPer100g REAL NOT NULL,
    ProteinPer100g  REAL NOT NULL,
    CarbsPer100g    REAL NOT NULL,
    FatPer100g      REAL NOT NULL,
    IsLocal         BIT NOT NULL CONSTRAINT DF_Foods_IsLocal DEFAULT (0)
);

CREATE TABLE dbo.NutritionLogs (
    Id        INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_NutritionLogs PRIMARY KEY,
    UserId    INT NOT NULL,
    FoodId    INT NOT NULL,
    Grams     REAL NOT NULL,
    [Date]    DATETIME2 NOT NULL CONSTRAINT DF_NutritionLogs_Date DEFAULT (SYSUTCDATETIME()),
    MealType  NVARCHAR(100) NOT NULL CONSTRAINT DF_NutritionLogs_MealType DEFAULT (N'Lunch'),
    CONSTRAINT FK_NutritionLogs_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE,
    CONSTRAINT FK_NutritionLogs_Foods_FoodId FOREIGN KEY (FoodId)
        REFERENCES dbo.Foods (Id) ON DELETE CASCADE
);
CREATE INDEX IX_NutritionLogs_UserId ON dbo.NutritionLogs (UserId);
CREATE INDEX IX_NutritionLogs_FoodId ON dbo.NutritionLogs (FoodId);

CREATE TABLE dbo.WorkoutPlans (
    Id               INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_WorkoutPlans PRIMARY KEY,
    Name             NVARCHAR(255) NOT NULL,
    [Description]    NVARCHAR(MAX) NOT NULL,
    Difficulty       NVARCHAR(100) NOT NULL CONSTRAINT DF_WorkoutPlans_Difficulty DEFAULT (N'Beginner'),
    CreatedByUserId  INT NOT NULL,
    CONSTRAINT FK_WorkoutPlans_Users_CreatedByUserId FOREIGN KEY (CreatedByUserId)
        REFERENCES dbo.Users (Id) ON DELETE NO ACTION
);
CREATE INDEX IX_WorkoutPlans_CreatedByUserId ON dbo.WorkoutPlans (CreatedByUserId);

CREATE TABLE dbo.WorkoutLogs (
    Id              INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_WorkoutLogs PRIMARY KEY,
    UserId          INT NOT NULL,
    WorkoutPlanId   INT NOT NULL,
    [Date]          DATETIME2 NOT NULL CONSTRAINT DF_WorkoutLogs_Date DEFAULT (SYSUTCDATETIME()),
    DurationMinutes INT NOT NULL,
    CaloriesBurned  INT NOT NULL,
    Notes           NVARCHAR(MAX) NULL,
    CONSTRAINT FK_WorkoutLogs_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE,
    CONSTRAINT FK_WorkoutLogs_WorkoutPlans_WorkoutPlanId FOREIGN KEY (WorkoutPlanId)
        REFERENCES dbo.WorkoutPlans (Id) ON DELETE CASCADE
);
CREATE INDEX IX_WorkoutLogs_UserId ON dbo.WorkoutLogs (UserId);
CREATE INDEX IX_WorkoutLogs_WorkoutPlanId ON dbo.WorkoutLogs (WorkoutPlanId);

CREATE TABLE dbo.MoodLogs (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_MoodLogs PRIMARY KEY,
    UserId      INT NOT NULL,
    MoodScore   INT NOT NULL,
    EnergyScore INT NOT NULL,
    [Date]      DATETIME2 NOT NULL CONSTRAINT DF_MoodLogs_Date DEFAULT (SYSUTCDATETIME()),
    Note        NVARCHAR(MAX) NULL,
    CONSTRAINT FK_MoodLogs_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE
);
CREATE INDEX IX_MoodLogs_UserId ON dbo.MoodLogs (UserId);

CREATE TABLE dbo.Badges (
    Id          INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Badges PRIMARY KEY,
    UserId      INT NOT NULL,
    Name        NVARCHAR(255) NOT NULL,
    Icon        NVARCHAR(255) NOT NULL CONSTRAINT DF_Badges_Icon DEFAULT (N'🏅'),
    [Description] NVARCHAR(MAX) NOT NULL,
    EarnedAt    DATETIME2 NOT NULL CONSTRAINT DF_Badges_EarnedAt DEFAULT (SYSUTCDATETIME()),
    CONSTRAINT FK_Badges_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE
);
CREATE INDEX IX_Badges_UserId ON dbo.Badges (UserId);

CREATE TABLE dbo.TwoFactorTokens (
    Id           INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_TwoFactorTokens PRIMARY KEY,
    UserId       INT NOT NULL,
    Purpose      NVARCHAR(50) NOT NULL,
    CodeHash     NVARCHAR(200) NOT NULL,
    Salt         NVARCHAR(200) NOT NULL,
    CreatedAt    DATETIME2 NOT NULL,
    ExpiresAt    DATETIME2 NOT NULL,
    AttemptCount INT NOT NULL CONSTRAINT DF_TwoFactorTokens_AttemptCount DEFAULT(0),
    ConsumedAt   DATETIME2 NULL,
    CONSTRAINT FK_TwoFactorTokens_Users_UserId FOREIGN KEY (UserId)
        REFERENCES dbo.Users (Id) ON DELETE CASCADE
);
CREATE INDEX IX_TwoFactorTokens_User_Expires ON dbo.TwoFactorTokens (UserId, ExpiresAt DESC);

GO

