using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Login;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using orienteering_backend.Core.Domain.Login;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Web;
//using System.Web.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authorization;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Authentication.Services;

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

    public async Task<User> CreateUser(User user)
    {
        //endre denne (input?) user til createUser?, ikke bryt ddd 


        //Console.WriteLine("create user");
        //if (!ModelState.IsValid)
        //{
        //    Console.WriteLine("modelstate not valid");
        //    return BadRequest(ModelState);
        //}

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
                
            Console.WriteLine("not successfull added");
            return null;
        }

        Console.WriteLine($"added successfully\n px: {user.Password}");

        user.Password = null;
        return user;

    }

   

    public async Task<User> SignInUser(User user)
    {
        //bør sjekke om forrige bruker skal logges ut før du logger inn!!



        //Console.WriteLine(HttpContent.)
        //Console.WriteLine("login user");
        //if (!ModelState.IsValid) { return BadRequest(ModelState); }
        //Console.WriteLine("a");


        //Console.WriteLine($"username {user.UserName}\n password {user.Password}\n\n");

        var testuser = await _userManager.FindByNameAsync(user.UserName);
        //var test2 = await _userManager.CheckPasswordAsync(
        //    testuser,
        //    user.Password
        //    );
        //Console.WriteLine("\ncorrect password?");

        //Console.WriteLine(test2.ToString());


        //var testing = await _signInManager.CanSignInAsync(
        //    user: new IdentityUser(
        //            userName: user.UserName
        //        )
        //    );
        //Console.WriteLine("\n har lov til å logge inn??");
        //Console.WriteLine(testing.ToString());




        //denne er vanskeligere??
        var result = await _signInManager.PasswordSignInAsync(

            //var result = await _signInManager.CheckPasswordSignInAsync(
            testuser,
            user.Password,
            false,
            false

        );





        Console.WriteLine("\nhar prøvd å logge inn\n");

        if (!result.Succeeded)
        {
            Console.WriteLine("problem!!\n");
            Console.WriteLine(result.Succeeded);
            return null;

        }
        Console.WriteLine("\nok login!!");

        //_signInManager.IsSignedIn(testuser);
        

       // //sjekk om logget inn
       // //var ferdig = await _userManager.is
       // //Console.WriteLine("er logget inn nå? ");

       // Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);
       //// testuser.is
       
       // var ferdig = testuser.Identity.IsAuthenticated;
       // Console.WriteLine(ferdig);
       // bool val1 = HttpContext.Current.User.Identity.IsAuthenticated;


       // Console.WriteLine("er logget inn nå? ");
       // //denne gir ut brukernavn
       // Console.WriteLine(User.Identity.Name);

       // //dette er innlogget bruker
       // Console.WriteLine(_userManager.GetUserId(HttpContext.User));


       // Console.WriteLine(User.Identity.ToString());
       // var claimsUser = User.Identity.GetType();
       // Console.WriteLine(claimsUser.GUID);
       // //Console.WriteLine(_userManager.GetUserId(claimsUser));
       // Console.WriteLine(_userManager.GetUserAsync(HttpContext.User));

       // Console.WriteLine(User.Identity.IsAuthenticated);

        return user;


    }

    public async Task SignOutUser()
    {
        await _signInManager.SignOutAsync();
        return;//Task.CompletedTask;


    }
}

