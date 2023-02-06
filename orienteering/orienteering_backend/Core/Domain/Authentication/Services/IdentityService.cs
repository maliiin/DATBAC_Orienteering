using Microsoft.AspNetCore.Identity;


namespace orienteering_backend.Core.Domain.Authentication.Services;

public class IdentityService :IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    public IdentityService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<UserRegistration> CreateUser(UserRegistration user)
    {
        //endre denne (input?) user til createUser?, ikke bryt ddd 

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
}

