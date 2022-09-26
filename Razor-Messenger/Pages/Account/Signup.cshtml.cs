using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Razor_Messenger.Pages.Account;

public class Signup : PageModel
{
    [BindProperty]
    public SignUpCredentialsVm Credentials { get; set; }
    
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

public class SignUpCredentialsVm
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
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}