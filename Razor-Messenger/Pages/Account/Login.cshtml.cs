using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Pages.Account;

public class Login : PageModel
{
    [BindProperty] public LoginCredentialsVm Credentials { get; set; }
    
    private readonly IAuthService _authService;

    public Login(IAuthService authService)
    {
        _authService = authService;
    }
    
    public void OnGet()
    {

    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        User user;
        try
        {
            user = _authService.Login(Credentials.Username, Credentials.Password);
        }
        catch (Exception e)
        {
            if (e is not InvalidCredentialsException)
                return RedirectToPage("/Error");
            
            ModelState.AddModelError("Credentials.Username", "Invalid username or password!");
            return Page();
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.NameIdentifier, user.Username)
        };
        var identity = new ClaimsIdentity(claims, "PizzaSlice");
        var principal = new ClaimsPrincipal(identity);
        
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = Credentials.RememberMe
        };
        
        await HttpContext.SignInAsync("PizzaSlice", principal, authProperties);

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