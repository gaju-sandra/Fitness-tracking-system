using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace fit.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CaloriesPer100g = table.Column<float>(type: "real", nullable: false),
                    ProteinPer100g = table.Column<float>(type: "real", nullable: false),
                    CarbsPer100g = table.Column<float>(type: "real", nullable: false),
                    FatPer100g = table.Column<float>(type: "real", nullable: false),
                    IsLocal = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Province = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Sector = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Difficulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: true),
                    TrainerId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Badges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EarnedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Badges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Badges_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BodyWeights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyWeights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BodyWeights_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TargetValue = table.Column<float>(type: "real", nullable: false),
                    CurrentValue = table.Column<float>(type: "real", nullable: false),
                    Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    PreferredTrainerGender = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MoodLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    MoodScore = table.Column<int>(type: "int", nullable: false),
                    EnergyScore = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoodLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoodLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientUserId = table.Column<int>(type: "int", nullable: true),
                    RecipientRole = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_RecipientUserId",
                        column: x => x.RecipientUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NutritionLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    Grams = table.Column<float>(type: "real", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MealType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NutritionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NutritionLogs_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NutritionLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationMinutes = table.Column<int>(type: "int", nullable: false),
                    CaloriesBurned = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutLogs_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "CaloriesPer100g", "CarbsPer100g", "Category", "FatPer100g", "IsLocal", "Name", "ProteinPer100g" },
                values: new object[,]
                {
                    { 1, 360f, 79f, "Grains", 1f, true, "Ugali", 3f },
                    { 2, 45f, 6f, "Vegetables", 1f, true, "Isombe (Cassava Leaves)", 4f },
                    { 3, 77f, 17f, "Vegetables", 0.1f, true, "Ibirayi (Potatoes)", 2f },
                    { 4, 347f, 63f, "Legumes", 1f, true, "Beans (Ibishyimbo)", 22f },
                    { 5, 165f, 0f, "Protein", 3.6f, false, "Chicken Breast", 31f },
                    { 6, 89f, 23f, "Fruits", 0.3f, true, "Banana (Igitoki)", 1.1f },
                    { 7, 61f, 4.8f, "Dairy", 3.3f, true, "Milk (Amata)", 3.2f },
                    { 8, 86f, 20f, "Vegetables", 0.1f, true, "Sweet Potato (Ibijumba)", 1.6f }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Trainer" },
                    { 3, "User" }
                });

            migrationBuilder.InsertData(
                table: "WorkoutPlans",
                columns: new[] { "Id", "CreatedByUserId", "Description", "Difficulty", "Name" },
                values: new object[,]
                {
                    { 1, 2, "30-min morning run and stretching", "Beginner", "Morning Cardio" },
                    { 2, 2, "Full body strength training", "Intermediate", "Strength Builder" },
                    { 3, 2, "High intensity interval training", "Advanced", "HIIT Blast" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Gender", "LocationId", "Name", "PasswordHash", "RoleId", "TrainerId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "admin@fittrack.rw", "Male", null, "Admin User", "$2a$11$q1WSoNCn3OW9yZH1mlnhWO0aS7PoNqpuv.BYZtQX789/AMsv2nCsy", 1, null },
                    { 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "trainer@fittrack.rw", "Male", null, "Jean Trainer", "$2a$11$LD90HXmSKZHpvrQbFcNjU.B.bXbYsRT8qS2EZyAyxB7WSoYzLDZuu", 2, null },
                    { 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "alice@fittrack.rw", "Female", null, "Alice Uwase", "$2a$11$n1Xo51XXkQy2ABL65mkyWeKwTtG1kFX4urXvANI/.1JA8SgTW9PLm", 3, 2 },
                    { 4, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "bob@fittrack.rw", "Male", null, "Bob Mugisha", "$2a$11$MyhOo8KzwMizE9U6HzYmbeQlPW0az0tgoVQwxHwx/qKMQZeCW/Cf2", 3, 2 }
                });

            migrationBuilder.InsertData(
                table: "Badges",
                columns: new[] { "Id", "Description", "EarnedAt", "Icon", "Name", "UserId" },
                values: new object[,]
                {
                    { 1, "Completed your first workout", new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "🏋️", "First Workout", 3 },
                    { 2, "Worked out 3 days in a row", new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "🔥", "3-Day Streak", 3 }
                });

            migrationBuilder.InsertData(
                table: "BodyWeights",
                columns: new[] { "Id", "Date", "Note", "UserId", "Weight" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Starting weight", 3, 72.5f },
                    { 2, new DateTime(2025, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 71.8f },
                    { 3, new DateTime(2025, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 71.2f },
                    { 4, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 3, 70.5f },
                    { 5, new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 85f },
                    { 6, new DateTime(2025, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 4, 84.2f }
                });

            migrationBuilder.InsertData(
                table: "Goals",
                columns: new[] { "Id", "CurrentValue", "Deadline", "IsCompleted", "PreferredTrainerGender", "TargetValue", "Title", "Type", "UserId" },
                values: new object[,]
                {
                    { 1, 70.5f, new DateTime(2025, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Any", 67.5f, "Lose 5kg", "Weight", 3 },
                    { 2, 3f, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Any", 5f, "Run 5km daily", "Workout", 3 },
                    { 3, 84.2f, new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Any", 90f, "Build muscle mass", "Weight", 4 }
                });

            migrationBuilder.InsertData(
                table: "MoodLogs",
                columns: new[] { "Id", "Date", "EnergyScore", "MoodScore", "Note", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 4, "Feeling motivated!", 3 },
                    { 2, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 3, null, 3 }
                });

            migrationBuilder.InsertData(
                table: "NutritionLogs",
                columns: new[] { "Id", "Date", "FoodId", "Grams", "MealType", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 200f, "Lunch", 3 },
                    { 2, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 6, 150f, "Breakfast", 3 },
                    { 3, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 5, 150f, "Dinner", 3 }
                });

            migrationBuilder.InsertData(
                table: "WorkoutLogs",
                columns: new[] { "Id", "CaloriesBurned", "Date", "DurationMinutes", "Notes", "UserId", "WorkoutPlanId" },
                values: new object[,]
                {
                    { 1, 280, new DateTime(2025, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 35, "Felt great!", 3, 1 },
                    { 2, 250, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 30, null, 3, 1 },
                    { 3, 320, new DateTime(2025, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), 45, null, 3, 2 },
                    { 4, 400, new DateTime(2025, 5, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 60, null, 4, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Badges_UserId",
                table: "Badges",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BodyWeights_UserId",
                table: "BodyWeights",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_UserId",
                table: "Goals",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Province_Sector",
                table: "Locations",
                columns: new[] { "Province", "Sector" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MoodLogs_UserId",
                table: "MoodLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_RecipientUserId_IsRead_CreatedAt",
                table: "Notifications",
                columns: new[] { "RecipientUserId", "IsRead", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_NutritionLogs_FoodId",
                table: "NutritionLogs",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_NutritionLogs_UserId",
                table: "NutritionLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocationId",
                table: "Users",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_UserId",
                table: "WorkoutLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutLogs_WorkoutPlanId",
                table: "WorkoutLogs",
                column: "WorkoutPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Badges");

            migrationBuilder.DropTable(
                name: "BodyWeights");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "MoodLogs");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NutritionLogs");

            migrationBuilder.DropTable(
                name: "WorkoutLogs");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
