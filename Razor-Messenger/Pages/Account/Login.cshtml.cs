using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Messenger.Pages.Account;

public class Login : PageModel
{
    [BindProperty] public LoginCredentialsVm Credentials { get; set; }

    public void OnGet()
    {

    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        return RedirectToPage("/Index");
    }
}

public class LoginCredentialsVm
{
    [Required]
    [Display(Name = "Username")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 20 characters")]
    [RegularExpression(@"[\w]+", ErrorMessage = "Username can only contain letters, numbers and underscores")]
    public string Username { get; set; }

    [Required]
    [Display(Name = "Password")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string Password { get; set; }

    [Display(Name = "Remember me")] public bool RememberMe { get; set; }
}