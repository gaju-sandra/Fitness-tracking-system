using fit.Data;
using fit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages.Admin;

public class UsersModel(AppDbContext db) : BasePageModel
{
    public List<fit.Models.User> Users { get; set; } = [];
    public List<Role> Roles { get; set; } = [];
    [BindProperty] public fit.Models.User EditUser { get; set; } = new();
    [BindProperty] public string? NewPassword { get; set; }
    public string? Message { get; set; }
    public bool ShowUserModal { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;
        Users = await db.Users.Include(u => u.Role).OrderBy(u => u.Name).ToListAsync();
        Roles = await db.Roles.ToListAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostSaveAsync()
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;

        var namingError = await ValidateRoleNameConventionAsync(EditUser.Name, EditUser.RoleId);
        if (!string.IsNullOrEmpty(namingError))
        {
            Message = namingError;
            return await ReloadAsync();
        }

        if (EditUser.Id == 0)
        {
            if (string.IsNullOrEmpty(NewPassword)) { Message = "Password required for new user."; return await ReloadAsync(); }
            EditUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            db.Users.Add(EditUser);
            Message = "User created successfully.";
        }
        else
        {
            var existing = await db.Users.FindAsync(EditUser.Id);
            if (existing == null) return NotFound();
            existing.Name = EditUser.Name;
            existing.Email = EditUser.Email;
            existing.RoleId = EditUser.RoleId;
            existing.TrainerId = EditUser.TrainerId;
            if (!string.IsNullOrEmpty(NewPassword))
                existing.PasswordHash = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            Message = "User updated successfully.";
        }
        await db.SaveChangesAsync();
        TempData["Message"] = Message;
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var r = RequireRole("Admin"); if (r is RedirectToPageResult) return r;
        var user = await db.Users.FindAsync(id);
        if (user != null && user.Id != SessionUserId)
        {
            db.Users.Remove(user);
            await db.SaveChangesAsync();
            TempData["Message"] = "User deleted.";
        }
        return RedirectToPage();
    }

    private async Task<PageResult> ReloadAsync()
    {
        ShowUserModal = true;
        Users = await db.Users.Include(u => u.Role).OrderBy(u => u.Name).ToListAsync();
        Roles = await db.Roles.ToListAsync();
        return Page();
    }

    private async Task<string?> ValidateRoleNameConventionAsync(string? name, int roleId)
    {
        var roleName = await db.Roles.Where(r => r.Id == roleId).Select(r => r.Name).FirstOrDefaultAsync();
        if (string.IsNullOrWhiteSpace(roleName)) return "Selected role is invalid.";

        var normalizedName = (name ?? string.Empty).Trim();
        if (roleName == "Admin" && !normalizedName.EndsWith("Admin", StringComparison.OrdinalIgnoreCase))
            return "Admin name must end with 'Admin' (example: GajuAdmin).";

        if (roleName == "Trainer" && !normalizedName.Contains("Trainer", StringComparison.OrdinalIgnoreCase))
            return "Trainer name must include 'Trainer' (example: GajuTrainer).";

        return null;
    }
}
