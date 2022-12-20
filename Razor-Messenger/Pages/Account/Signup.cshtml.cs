using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Pages.Account;

public class Signup : PageModel
{
    [BindProperty]
    public SignUpCredentialsVm Credentials { get; set; }
    
    private readonly UserManager<User> _userManager;

    public Signup(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();
        
        if (string.IsNullOrEmpty(Credentials.DisplayName))
            Credentials.DisplayName = Credentials.Username;

        var result = await _userManager.CreateAsync(
            new User { UserName = Credentials.Username, DisplayName = Credentials.DisplayName },
            Credentials.Password
        );

        if (!result.Succeeded)
        {
            ModelState.AddModelError("Credentials.Username", "This username is already taken");
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, Credentials.DisplayName),
            new(ClaimTypes.NameIdentifier, Credentials.Username)
        };
        var identity = new ClaimsIdentity(claims, "PizzaSlice");
        var principal = new ClaimsPrincipal(identity);
        
        await HttpContext.SignInAsync("PizzaSlice", principal);

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
    
    [Display(Name = "Display Name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Display name must be between 3 and 20 characters")]
    public string DisplayName { get; set; }

    [Required]
    [Display(Name = "Password")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string Password { get; set; }
    
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }
}