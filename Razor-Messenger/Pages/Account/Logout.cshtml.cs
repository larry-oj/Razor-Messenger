using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Messenger.Pages.Account;

public class Logout : PageModel
{
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync("PizzaSlice");
        return RedirectToPage("/Index");
    }
}