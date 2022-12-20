using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<User> _userManager;
    private readonly IHubContext<UserListHub, IUserListClient> _hub;
    
    public string Username => 
        User.FindFirstValue(ClaimTypes.NameIdentifier);

    [BindProperty(SupportsGet = true)] 
    public UserVM UserVM { get; set; }
    
    [BindProperty] 
    public PasswordVM PasswordVM { get; set; }

    public Profile(UserManager<User> userManager,
        IHubContext<UserListHub, IUserListClient> hub)
    {
        _userManager = userManager;
        _hub = hub;
    }
    
    public async Task OnGet()
    {
        var user = await _userManager.FindByNameAsync(Username);
        UserVM = new UserVM(user);
    }

    public async Task<IActionResult> OnPostUpdateProfileAsync()
    {
        if (!ModelState["UserVM.DisplayName"]!.ValidationState.Equals(ModelValidationState.Valid))
            return Page();

        var user = await _userManager.FindByNameAsync(Username);
        user.DisplayName = UserVM.DisplayName;
        await _userManager.UpdateAsync(user);
        
        await _hub.Clients.All.UpdateDisplayName(Username, UserVM.DisplayName);
        
        return Page();
    }
    
    public async Task<IActionResult> OnPostUpdatePasswordAsync()
    {
        var user = await _userManager.FindByNameAsync(Username);
        UserVM = new UserVM(user);
        
        ModelState.Remove("DisplayName");
        if (!ModelState.IsValid)
            return Page();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, PasswordVM.NewPassword);

        if (!result.Succeeded)
            ModelState.AddModelError("PasswordVM.OldPassword", "Password is incorrect");

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