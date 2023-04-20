using Microsoft.AspNetCore.Identity;

namespace orienteering_backend.Core.Domain.Authentication.Services;


public class IdentityService :IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public IdentityService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CreateUser(UserRegistration user)
    {

        var result = await _userManager.CreateAsync(
            new IdentityUser()
            {
                UserName = user.UserName,
                Email = user.Email
            },
            user.Password
        );

        if (!result.Succeeded)
        {   
            return false;
        }

        return true;
    }

   
    public async Task<bool> SignInUser(UserSignIn inpUser)
    {
        var user = await _userManager.FindByNameAsync(inpUser.UserName);
        if (user == null) {
            return false;
        }
        var result = await _signInManager.PasswordSignInAsync(
            user,
            inpUser.Password,
            false,
            false
        );

        if (!result.Succeeded)
        {
            return false;
        }
        return true;
    }

    public async Task SignOutUser()
    {
        await _signInManager.SignOutAsync();
        return;
    }

    public  Guid? GetCurrentUserId()
    {
        HttpContext context = _httpContextAccessor.HttpContext;

        var id = context.User.Claims.FirstOrDefault();
        if (id == null) { return null; }

        return new Guid(id.Value);

    }
}

