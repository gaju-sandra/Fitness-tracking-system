using fit.Data;
using fit.Models;
using fit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace fit.Pages.Auth;

public class TwoFactorModel(AppDbContext db, IEmailSender emailSender) : BasePageModel
{
    private const string PendingRegNameKey = "PendingReg:Name";
    private const string PendingRegEmailKey = "PendingReg:Email";
    private const string PendingRegPasswordHashKey = "PendingReg:PasswordHash";
    private const string PendingRegRoleIdKey = "PendingReg:RoleId";
    private const string PendingRegOtpHashKey = "PendingReg:OtpHash";
    private const string PendingRegOtpSaltKey = "PendingReg:OtpSalt";
    private const string PendingRegOtpExpiresKey = "PendingReg:OtpExpires";
    private const string PendingLoginUserIdKey = "PendingLogin:UserId";
    private const string PendingLoginOtpHashKey = "PendingLogin:OtpHash";
    private const string PendingLoginOtpSaltKey = "PendingLogin:OtpSalt";
    private const string PendingLoginOtpExpiresKey = "PendingLogin:OtpExpires";

    [BindProperty] public string Code { get; set; } = "";
    [FromQuery] public string Purpose { get; set; } = "";

    public string? MaskedEmail { get; set; }
    public string? Error { get; set; }
    public string? Info { get; set; }

    public IActionResult OnGet()
    {
        if (string.Equals(Purpose, "register", StringComparison.OrdinalIgnoreCase))
        {
            var regEmail = HttpContext.Session.GetString(PendingRegEmailKey);
            if (string.IsNullOrWhiteSpace(regEmail))
                return RedirectToPage("/Auth/Register");

            MaskedEmail = MaskEmail(regEmail);
            return Page();
        }

        if (string.Equals(Purpose, "login", StringComparison.OrdinalIgnoreCase))
        {
            var loginUserId = HttpContext.Session.GetInt32(PendingLoginUserIdKey);
            if (!loginUserId.HasValue)
                return RedirectToPage("/Auth/Login");

            var email = db.Users.Where(u => u.Id == loginUserId.Value).Select(u => u.Email).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(email))
                return RedirectToPage("/Auth/Login");

            MaskedEmail = MaskEmail(email);
            return Page();
        }

        return RedirectToPage("/Auth/Login");
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.Equals(Purpose, "login", StringComparison.OrdinalIgnoreCase))
            return await VerifyLoginOtpAsync();

        if (!string.Equals(Purpose, "register", StringComparison.OrdinalIgnoreCase)) return RedirectToPage("/Auth/Login");

        var name = HttpContext.Session.GetString(PendingRegNameKey);
        var email = HttpContext.Session.GetString(PendingRegEmailKey);
        var passwordHash = HttpContext.Session.GetString(PendingRegPasswordHashKey);
        var roleId = HttpContext.Session.GetInt32(PendingRegRoleIdKey);
        var otpHash = HttpContext.Session.GetString(PendingRegOtpHashKey);
        var otpSalt = HttpContext.Session.GetString(PendingRegOtpSaltKey);
        var otpExpires = HttpContext.Session.GetString(PendingRegOtpExpiresKey);

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordHash) ||
            !roleId.HasValue || string.IsNullOrWhiteSpace(otpHash) || string.IsNullOrWhiteSpace(otpSalt) || string.IsNullOrWhiteSpace(otpExpires))
        {
            return RedirectToPage("/Auth/Register");
        }

        MaskedEmail = MaskEmail(email);

        if (string.IsNullOrWhiteSpace(Code) || Code.Length != 6)
        {
            Error = "Enter the 6-digit OTP code.";
            return Page();
        }

        if (!DateTime.TryParse(otpExpires, out var expiresAt) || expiresAt < DateTime.UtcNow)
        {
            Error = "OTP has expired. Click resend to get a new code.";
            return Page();
        }

        if (!BCrypt.Net.BCrypt.Verify(Code.Trim() + otpSalt, otpHash))
        {
            Error = "Invalid OTP code.";
            return Page();
        }

        if (await db.Users.AnyAsync(u => u.Email == email))
        {
            ClearPendingRegistration();
            return RedirectToPage("/Auth/Login");
        }

        db.Users.Add(new fit.Models.User
        {
            Name = name,
            Email = email,
            PasswordHash = passwordHash,
            RoleId = roleId.Value
        });

        await db.SaveChangesAsync();
        ClearPendingRegistration();
        TempData["Message"] = "Account created successfully. Please login.";
        return RedirectToPage("/Auth/Login");
    }

    public async Task<IActionResult> OnPostResendAsync()
    {
        if (string.Equals(Purpose, "login", StringComparison.OrdinalIgnoreCase))
            return await ResendLoginOtpAsync();

        if (!string.Equals(Purpose, "register", StringComparison.OrdinalIgnoreCase))
            return RedirectToPage("/Auth/Login");

        var email = HttpContext.Session.GetString(PendingRegEmailKey);
        var name = HttpContext.Session.GetString(PendingRegNameKey) ?? "there";
        if (string.IsNullOrWhiteSpace(email))
            return RedirectToPage("/Auth/Register");

        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var hash = BCrypt.Net.BCrypt.HashPassword(code + salt);

        HttpContext.Session.SetString(PendingRegOtpHashKey, hash);
        HttpContext.Session.SetString(PendingRegOtpSaltKey, salt);
        HttpContext.Session.SetString(PendingRegOtpExpiresKey, DateTime.UtcNow.AddMinutes(10).ToString("O"));

        try
        {
            var html = $"""
                        <h2>FitTrack Verification Code</h2>
                        <p>Hi {name},</p>
                        <p>Your new OTP code is: <strong style='font-size:22px'>{code}</strong></p>
                        <p>This code expires in 10 minutes.</p>
                        """;
            await emailSender.SendAsync(email, "Your FitTrack OTP Code", html);
            Info = "A new OTP has been sent.";
        }
        catch
        {
            Error = "Unable to resend OTP right now. Please try again.";
        }

        MaskedEmail = MaskEmail(email);
        return Page();
    }

    private async Task<IActionResult> VerifyLoginOtpAsync()
    {
        var loginUserId = HttpContext.Session.GetInt32(PendingLoginUserIdKey);
        var otpHash = HttpContext.Session.GetString(PendingLoginOtpHashKey);
        var otpSalt = HttpContext.Session.GetString(PendingLoginOtpSaltKey);
        var otpExpires = HttpContext.Session.GetString(PendingLoginOtpExpiresKey);

        if (!loginUserId.HasValue || string.IsNullOrWhiteSpace(otpHash) || string.IsNullOrWhiteSpace(otpSalt) || string.IsNullOrWhiteSpace(otpExpires))
            return RedirectToPage("/Auth/Login");

        var user = await db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == loginUserId.Value);
        if (user == null)
            return RedirectToPage("/Auth/Login");

        await NormalizeRoleByNameConventionAsync(user);

        MaskedEmail = MaskEmail(user.Email);

        if (string.IsNullOrWhiteSpace(Code) || Code.Length != 6)
        {
            Error = "Enter the 6-digit OTP code.";
            return Page();
        }

        if (!DateTime.TryParse(otpExpires, out var expiresAt) || expiresAt < DateTime.UtcNow)
        {
            Error = "OTP has expired. Click resend to get a new code.";
            return Page();
        }

        if (!BCrypt.Net.BCrypt.Verify(Code.Trim() + otpSalt, otpHash))
        {
            Error = "Invalid OTP code.";
            return Page();
        }

        ClearPendingLogin();
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role?.Name ?? "User");
        return RedirectToPage("/Dashboard/Index");
    }

    private async Task<IActionResult> ResendLoginOtpAsync()
    {
        var loginUserId = HttpContext.Session.GetInt32(PendingLoginUserIdKey);
        if (!loginUserId.HasValue)
            return RedirectToPage("/Auth/Login");

        var user = await db.Users.FirstOrDefaultAsync(u => u.Id == loginUserId.Value);
        if (user == null)
            return RedirectToPage("/Auth/Login");

        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var hash = BCrypt.Net.BCrypt.HashPassword(code + salt);

        HttpContext.Session.SetString(PendingLoginOtpHashKey, hash);
        HttpContext.Session.SetString(PendingLoginOtpSaltKey, salt);
        HttpContext.Session.SetString(PendingLoginOtpExpiresKey, DateTime.UtcNow.AddMinutes(10).ToString("O"));

        try
        {
            var html = $"""
                        <h2>FitTrack Login Verification</h2>
                        <p>Hi {user.Name},</p>
                        <p>Your new login OTP code is: <strong style='font-size:22px'>{code}</strong></p>
                        <p>This code expires in 10 minutes.</p>
                        """;
            await emailSender.SendAsync(user.Email, "Your FitTrack Login OTP", html);
            Info = "A new OTP has been sent.";
        }
        catch
        {
            Error = "Unable to resend OTP right now. Please try again.";
        }

        MaskedEmail = MaskEmail(user.Email);
        return Page();
    }

    private void ClearPendingRegistration()
    {
        HttpContext.Session.Remove(PendingRegNameKey);
        HttpContext.Session.Remove(PendingRegEmailKey);
        HttpContext.Session.Remove(PendingRegPasswordHashKey);
        HttpContext.Session.Remove(PendingRegRoleIdKey);
        HttpContext.Session.Remove(PendingRegOtpHashKey);
        HttpContext.Session.Remove(PendingRegOtpSaltKey);
        HttpContext.Session.Remove(PendingRegOtpExpiresKey);
    }

    private void ClearPendingLogin()
    {
        HttpContext.Session.Remove(PendingLoginUserIdKey);
        HttpContext.Session.Remove(PendingLoginOtpHashKey);
        HttpContext.Session.Remove(PendingLoginOtpSaltKey);
        HttpContext.Session.Remove(PendingLoginOtpExpiresKey);
    }

    private static string MaskEmail(string email)
    {
        var at = email.IndexOf('@');
        if (at <= 1) return email;
        var first = email[0];
        var domain = email[at..];
        return $"{first}***{domain}";
    }

    private async Task NormalizeRoleByNameConventionAsync(fit.Models.User user)
    {
        var roleName = "User";
        if (user.Name.EndsWith("Admin", StringComparison.OrdinalIgnoreCase))
            roleName = "Admin";
        else if (user.Name.Contains("Trainer", StringComparison.OrdinalIgnoreCase))
            roleName = "Trainer";

        var desiredRole = await db.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
        if (desiredRole == null) return;

        if (user.RoleId != desiredRole.Id)
        {
            user.RoleId = desiredRole.Id;
            await db.SaveChangesAsync();
        }

        user.Role = desiredRole;
    }
}

