using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace fit.Pages;

public class BasePageModel : PageModel
{
    public int? SessionUserId => HttpContext.Session.GetInt32("UserId");
    public string? SessionUserName => HttpContext.Session.GetString("UserName");
    public string? SessionUserRole => HttpContext.Session.GetString("UserRole");

    public bool IsLoggedIn => SessionUserId.HasValue;
    public bool IsAdmin => SessionUserRole == "Admin";
    public bool IsTrainer => SessionUserRole == "Trainer";
    public bool IsUser => SessionUserRole == "User";

    protected IActionResult RequireLogin()
    {
        if (!IsLoggedIn) return RedirectToPage("/Auth/Login");
        return Page();
    }

    protected IActionResult RequireRole(params string[] roles)
    {
        if (!IsLoggedIn) return RedirectToPage("/Auth/Login");
        if (!roles.Contains(SessionUserRole)) return RedirectToPage("/Dashboard/Index");
        return Page();
    }
}
