using fit.Data;
using fit.Services;
using fit.Models; // Added back to refer to User clearly
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace fit.Pages.Auth;

public class RegisterModel(AppDbContext db, IEmailSender emailSender) : BasePageModel
{
    private const string PendingRegNameKey = "PendingReg:Name";
    private const string PendingRegEmailKey = "PendingReg:Email";
    private const string PendingRegPasswordHashKey = "PendingReg:PasswordHash";
    private const string PendingRegRoleIdKey = "PendingReg:RoleId";
    private const string PendingRegOtpHashKey = "PendingReg:OtpHash";
    private const string PendingRegOtpSaltKey = "PendingReg:OtpSalt";
    private const string PendingRegOtpExpiresKey = "PendingReg:OtpExpires";

    [BindProperty] public string Name { get; set; } = "";
    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    [BindProperty] public string ConfirmPassword { get; set; } = "";
    [BindProperty] public int RoleId { get; set; } = 3;
    [BindProperty] public int? LocationId { get; set; }

    public string? Error { get; set; }
    public string? Success { get; set; }
    
    public List<SelectListItem> RoleOptions { get; set; } = [];
    public List<SelectListItem> LocationOptions { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        await LoadOptionsAsync();
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Password != ConfirmPassword)
        {
            Error = "Passwords do not match.";
            await LoadOptionsAsync();
            return Page();
        }

        if (await db.Users.AnyAsync(u => u.Email == Email))
        {
            Error = "Email is already registered.";
            await LoadOptionsAsync();
            return Page();
        }

        var selectedRoleId = await ResolveRoleIdFromNameConventionAsync(Name);
        if (selectedRoleId == 0)
        {
            Error = "Roles are not configured in the database. Add roles first, then try again.";
            await LoadOptionsAsync();
            return Page();
        }

        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var hash = BCrypt.Net.BCrypt.HashPassword(code + salt);

        HttpContext.Session.SetString(PendingRegNameKey, Name.Trim());
        HttpContext.Session.SetString(PendingRegEmailKey, Email.Trim().ToLowerInvariant());
        HttpContext.Session.SetString(PendingRegPasswordHashKey, BCrypt.Net.BCrypt.HashPassword(Password));
        HttpContext.Session.SetInt32(PendingRegRoleIdKey, selectedRoleId);
        HttpContext.Session.SetString(PendingRegOtpHashKey, hash);
        HttpContext.Session.SetString(PendingRegOtpSaltKey, salt);
        HttpContext.Session.SetString(PendingRegOtpExpiresKey, DateTime.UtcNow.AddMinutes(10).ToString("O"));

        try
        {
            var html = $"""
                        <h2>FitTrack Verification Code</h2>
                        <p>Hi {Name},</p>
                        <p>Your OTP code is: <strong style='font-size:22px'>{code}</strong></p>
                        <p>This code expires in 10 minutes.</p>
                        """;
            await emailSender.SendAsync(Email, "Your FitTrack OTP Code", html);
        }
        catch
        {
            Error = "Unable to send OTP email right now. Please verify SMTP settings and try again.";
            await LoadOptionsAsync();
            return Page();
        }

        return RedirectToPage("/Auth/TwoFactor", new { purpose = "register" });
    }

    private async Task LoadOptionsAsync()
    {
        RoleOptions = await db.Roles
            .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Name })
            .ToListAsync();
            
        LocationOptions = new List<SelectListItem>();
    }

    private async Task<int> ResolveRoleIdFromNameConventionAsync(string? name)
    {
        var normalizedName = (name ?? string.Empty).Trim();
        var roleName = "User";
        if (normalizedName.EndsWith("Admin", StringComparison.OrdinalIgnoreCase))
            roleName = "Admin";
        else if (normalizedName.Contains("Trainer", StringComparison.OrdinalIgnoreCase))
            roleName = "Trainer";

        return await db.Roles
            .Where(r => r.Name == roleName)
            .Select(r => r.Id)
            .FirstOrDefaultAsync();
    }
}
