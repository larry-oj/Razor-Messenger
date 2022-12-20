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

public class Login : PageModel
{
    [BindProperty] public LoginCredentialsVm Credentials { get; set; }
    public string ReturnUrl { get; set; }
    
    private readonly UserManager<User> _userManager;

    public Login(UserManager<User> userManager)
    {
        _userManager = userManager;
    }
    
    public void OnGet(string returnUrl)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPost()
    {
        if (!ModelState.IsValid)
            return Page();

        if (await _userManager.FindByNameAsync(Credentials.Username) is not { } user)
        {
            ModelState.AddModelError("Credentials.Username", "Invalid username or password!");
            return Page();
        }
        
        if (!await _userManager.CheckPasswordAsync(user, Credentials.Password))
        {
            ModelState.AddModelError("Credentials.Password", "Invalid username or password!");
            return Page();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.DisplayName),
            new(ClaimTypes.NameIdentifier, user.UserName)
        };
        var identity = new ClaimsIdentity(claims, "PizzaSlice");
        var principal = new ClaimsPrincipal(identity);
        
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = Credentials.RememberMe
        };
        
        await HttpContext.SignInAsync("PizzaSlice", principal, authProperties);

        return RedirectToPage(ReturnUrl ?? "/Index");
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