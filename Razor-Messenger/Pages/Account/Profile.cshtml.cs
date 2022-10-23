using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Razor_Messenger.Data.Models;
using Razor_Messenger.Hubs;
using Razor_Messenger.Services;
using Razor_Messenger.Services.Exceptions;

namespace Razor_Messenger.Pages.Account;

[Authorize]
public class Profile : PageModel
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly IHubContext<UserListHub, IUserListClient> _hub;
    
    public string Username => 
        User.FindFirstValue(ClaimTypes.NameIdentifier);

    [BindProperty(SupportsGet = true)] 
    public UserVM UserVM { get; set; }
    
    [BindProperty] 
    public PasswordVM PasswordVM { get; set; }

    public Profile(IUserService userService, 
        IAuthService authService,
        IHubContext<UserListHub, IUserListClient> hub)
    {
        _userService = userService;
        _authService = authService;
        _hub = hub;
    }
    
    public void OnGet()
    {
        var user = _userService.GetUser(Username);
        UserVM = new UserVM(user);
    }

    public async Task<IActionResult> OnPostUpdateProfileAsync()
    {
        if (!ModelState["UserVM.DisplayName"]!.ValidationState.Equals(ModelValidationState.Valid))
            return Page();
        
        await _userService.UpdateUserDisplayNameAsync(Username, UserVM.DisplayName);
        await _hub.Clients.All.UpdateDisplayName(Username, UserVM.DisplayName);
        
        return Page();
    }
    
    public IActionResult OnPostUpdatePasswordAsync()
    {
        UserVM = new UserVM(_userService.GetUser(Username));
        
        ModelState.Remove("DisplayName");
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var user = _authService.UpdatePassword(Username, PasswordVM.OldPassword, PasswordVM.NewPassword);
        }
        catch (Exception ex)
        {
            if (ex is not InvalidCredentialsException)
                return RedirectToPage("/Error");
            
            ModelState.AddModelError("PasswordVM.OldPassword", "Password is incorrect");
        }
        
        return Page();
    }
}

public class UserVM
{
    [Display(Name = "Display Name")]
    [StringLength(20, MinimumLength = 3, ErrorMessage = "Display name must be between 3 and 20 characters")]
    public string DisplayName { get; set; }

    public UserVM()
    {
    }

    public UserVM(User user) 
    {
        DisplayName = user.DisplayName;
    }
}

public class PasswordVM
{
    [Required]
    [Display(Name = "Old Password")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string OldPassword { get; set; }
    
    [Required]
    [Display(Name = "New Password")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 20 characters")]
    public string NewPassword { get; set; }
    
    [Required]
    [Display(Name = "Confirm")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}