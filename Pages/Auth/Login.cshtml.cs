using fit.Data;
using fit.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace fit.Pages.Auth;

public class LoginModel(AppDbContext db, IEmailSender emailSender) : BasePageModel
{
    private const string PendingLoginUserIdKey = "PendingLogin:UserId";
    private const string PendingLoginOtpHashKey = "PendingLogin:OtpHash";
    private const string PendingLoginOtpSaltKey = "PendingLogin:OtpSalt";
    private const string PendingLoginOtpExpiresKey = "PendingLogin:OtpExpires";

    [BindProperty] public string Email { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";

    public string? Error { get; set; }
    public string? Success { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await db.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.PasswordHash))
        {
            Error = "Invalid email or password.";
            return Page();
        }

        var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
        var salt = Convert.ToBase64String(RandomNumberGenerator.GetBytes(16));
        var hash = BCrypt.Net.BCrypt.HashPassword(code + salt);

        HttpContext.Session.SetInt32(PendingLoginUserIdKey, user.Id);
        HttpContext.Session.SetString(PendingLoginOtpHashKey, hash);
        HttpContext.Session.SetString(PendingLoginOtpSaltKey, salt);
        HttpContext.Session.SetString(PendingLoginOtpExpiresKey, DateTime.UtcNow.AddMinutes(10).ToString("O"));

        try
        {
            var html = $"""
                        <h2>FitTrack Login Verification</h2>
                        <p>Hi {user.Name},</p>
                        <p>Your login OTP code is: <strong style='font-size:22px'>{code}</strong></p>
                        <p>This code expires in 10 minutes.</p>
                        """;
            await emailSender.SendAsync(user.Email, "Your FitTrack Login OTP", html);
        }
        catch
        {
            Error = "Unable to send login OTP right now. Please try again.";
            return Page();
        }

        return RedirectToPage("/Auth/TwoFactor", new { purpose = "login" });
    }
}
