using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using fit.Data;
using fit.Models;
using Microsoft.EntityFrameworkCore;

namespace fit.Pages;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;
    public List<fit.Models.User> TopTrainers { get; set; } = [];

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        if (HttpContext.Session.GetInt32("UserId").HasValue)
            return RedirectToPage("/Dashboard/Index");

        TopTrainers = await _db.Users
            .Where(u => u.RoleId == 2)
            .Take(3)
            .ToListAsync();

        return Page();
    }
}
