using Microsoft.AspNetCore.Identity;
using System.Web;

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

    public async Task<UserRegistration> CreateUser(UserRegistration user)
    {
       

        //fix endre denne (input?) user til createUser?, ikke bryt ddd 

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
            return null;
        }

        user.Password = null;
        return user;

    }

   
    public async Task<UserSignIn> SignInUser(UserSignIn user)
    {
        var testuser = await _userManager.FindByNameAsync(user.UserName);
        //fix eroor handling- sjekk at den over ikke er null
        var result = await _signInManager.PasswordSignInAsync(
            testuser,
            user.Password,
            false,
            false
        );

        if (!result.Succeeded)
        {
            return null;
        }
        return user;
    }

    public async Task SignOutUser()
    {
        await _signInManager.SignOutAsync();
        return;
    }

    public  Guid? GetCurrentUserId()
    {
        HttpContext context = _httpContextAccessor.HttpContext;

        //gir userid, men exeption om ingen er logget inn
        //var p = HttpContext.User.Claims.First().Value;

        //gir userid, men er null om ingen er logget inn (klikker hvis du kjører .value når ingen er logget inn (null.value))            
        var id = context.User.Claims.FirstOrDefault();
        if (id == null) { return null; }

        return new Guid(id.Value);
        //System.Security.Claims.Claim id = HttpContext.User.Claims.FirstOrDefault();

    }
}

