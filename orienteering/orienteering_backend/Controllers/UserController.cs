﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authorization;
using orienteering_backend.Core.Domain.Authentication.Services;

namespace orienteering_backend.Controllers
{
    //[Route("api/[controller]/[action]")]
    //[RoutePrefix("api/user")]
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IIdentityService _identityService;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService, IIdentityService identityService)
        //public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _identityService = identityService;
        }

        //POST sign out

        [HttpPost("signOut")]
        public async Task SignOut()
        {
            Console.WriteLine("sign out");
            await _identityService.SignOutUser();
        }


        // POST: api/User
        //create user
        [HttpPost("createuser")]
        //[Route("api/user/createuser")]
        public async Task<ActionResult<User>> CreateUser(User user)
        //public string CreateUser()

        {
            Console.WriteLine("create user");
            //if (!ModelState.IsValid)
            //{
            //    Console.WriteLine("modelstate not valid");
            //    return BadRequest(ModelState);
            //}

            //var result = await _userManager.CreateAsync(
            //    new IdentityUser()
            //    {
            //        UserName = user.UserName,
            //        Email = user.Email
            //    },
            //    user.Password
            //);

            //if (!result.Succeeded)
            //{
            //    Console.WriteLine("not successfull added");
            //    return BadRequest(result.Errors);
            //}

            //Console.WriteLine($"added successfully\n px: {user.Password}");
            //user.Password = null;

            //return Created("", user);

            ////

            var createduser = await _identityService.CreateUser(user);
            if (createduser == null) { return BadRequest("problem creating the user"); }

            return Created("", createduser);



        }

        //POST
        //log in user
        [HttpPost("signinuser")]
        //[Route("api/user/signinuser")]
        public async Task<ActionResult<User>> SignInUser(User user)
        {
            ////bør sjekke om forrige bruker skal logges ut før du logger inn!!



            ////Console.WriteLine(HttpContent.)
            //Console.WriteLine("login user");
            //if (!ModelState.IsValid) { return BadRequest(ModelState); }
            //Console.WriteLine("a");


            //Console.WriteLine($"username {user.UserName}\n password {user.Password}\n\n");

            //var testuser = await _userManager.FindByNameAsync(user.UserName);
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


                        

            ////denne er vanskeligere??
            //var result = await _signInManager.PasswordSignInAsync(

            ////var result = await _signInManager.CheckPasswordSignInAsync(
            //    testuser,
            //    user.Password,
            //    false,
            //    false
                
            //);
            




            //Console.WriteLine("\nhar prøvd å logge inn\n");

            //if (!result.Succeeded)
            //{
            //    Console.WriteLine("problem!!\n");
            //    Console.WriteLine(result.Succeeded);
            //    return BadRequest(result);

            //}
            //Console.WriteLine("\nok login!!");




            ////sjekk om logget inn
            ////var ferdig=await _userManager.is
            //Console.WriteLine("er logget inn nå? ");

            //Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);
            ////testuser.is
            ////var ferdig=testuser.Identity.IsAuthenticated;
            ////Console.WriteLine(ferdig);
            ////bool val1 = HttpContext.Current.User.Identity.IsAuthenticated;


            //Console.WriteLine("er logget inn nå? ");
            ////denne gir ut brukernavn
            //Console.WriteLine(User.Identity.Name);

            ////dette er innlogget bruker
            //Console.WriteLine(_userManager.GetUserId(HttpContext.User));
            

            //Console.WriteLine(User.Identity.ToString());
            //var claimsUser = User.Identity.GetType();
            //Console.WriteLine(claimsUser.GUID);
            ////Console.WriteLine(_userManager.GetUserId(claimsUser));
            //Console.WriteLine(_userManager.GetUserAsync(HttpContext.User));

            //Console.WriteLine(User.Identity.IsAuthenticated);


            //Console.WriteLine(HttpContext.User.Identity.IsAuthenticated);

            //dette er userId til den som er logget inn!!
            //Console.WriteLine(HttpContext.User.Claims.First().Value);
            //er dette ok?
            var userSignedIn = await _identityService.SignInUser(user);
            if (userSignedIn == null) { return BadRequest(new string("could not sign in the user")); }
            return Ok("user signed in");
        }


        [HttpGet]
        [Route("GetSignedInUserId")]
        public ActionResult<string> GetSignedInUserId()
        {
            Console.WriteLine("is signed in???\n");
            
            //er en bruker logget in?
            var h = HttpContext.User.Identity.IsAuthenticated;

            //gir userid, men exeption om ingen er logget inn
            //var p = HttpContext.User.Claims.First().Value;

            //gir userid, men er null om ingen er logget inn (klikker hvis du kjører .value når ingen er logget inn (null.value))            
            var id = HttpContext.User.Claims.FirstOrDefault();
            Console.WriteLine($"is authenticated?? {h}");

            if (id is null) 
            { 
                Console.WriteLine("no user is signed in");
                return NotFound();

            }
            else 
            
            {

                Console.WriteLine($"value of id? {id.Value}");
                return id.Value;

            }

            //dette gir id om man er logget inn!!!
            //var p = HttpContext.User.Claims.FirstOrDefault().Value;





            //var t=HttpContext.User.Identity

            //_signInManager.IsSignedIn();


            //if (h!=null)
            //{
            //    _userManager.GetUserId(h);

            //}
        }

        /*
         bytte fra 1 til flere endpoint

            fungerer med registrering, men signInManager ødelegger!!!
            -url til controller
            -fetch url
            -setup proxy url
            -sign inn manager
         
         */

        //// GET: api/Users/username
        ////get userinfo from username
        //[HttpGet("{username}")]
        ////[Route("api/user/getuser")]


        ////testing authorization/authentication
        //[Authorize]

        //public async Task<ActionResult<User>> GetUser(string username)
        //{
        //    IdentityUser user = await _userManager.FindByNameAsync(username);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    //var user= new User
        //    //{
        //    //    UserName = user.UserName,
        //    //    Email = user.Email
        //    //};
        //    return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        //}


        ////kilde 2/2/23 https://www.endpointdev.com/blog/2022/06/implementing-authentication-in-asp.net-core-web-apis/
        //// POST: api/Users/BearerToken
        //[HttpPost("BearerToken")]
        //public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest("Bad credentials");
        //    }

        //    var user = await _userManager.FindByNameAsync(request.UserName);

        //    if (user == null)
        //    {
        //        return BadRequest("Bad credentials");
        //    }

        //    var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        //    if (!isPasswordValid)
        //    {
        //        return BadRequest("Bad credentials");
        //    }

        //    var token = _jwtService.CreateToken(user);

        //    return Ok(token);

        //}
    }
}
