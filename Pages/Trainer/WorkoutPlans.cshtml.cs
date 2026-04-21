using Microsoft.AspNetCore.Mvc;

namespace fit.Pages.Trainer;

public class WorkoutPlansModel : BasePageModel
{
    public IActionResult OnGet()
    {
        var r = RequireRole("Trainer", "Admin");
        if (r is RedirectToPageResult) return r;
        return RedirectToPage("/Admin/WorkoutPlans");
    }
}
