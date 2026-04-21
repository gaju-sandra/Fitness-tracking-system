using Microsoft.AspNetCore.Mvc;

namespace fit.Pages.Auth;

public class LogoutModel : BasePageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Clear();
        return RedirectToPage("/Index");
    }
}
