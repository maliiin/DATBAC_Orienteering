using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Web;


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

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService)
        //public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
        }


        // POST: api/User
        //create user
        [HttpPost("createuser")]
        //[Route("api/user/createuser")]
        public async Task<ActionResult<User>> CreateUser(User user)
        //public string CreateUser()

        {
            Console.WriteLine("create user");
            if (!ModelState.IsValid)
            {
                Console.WriteLine("modelstate not valid");
                return BadRequest(ModelState);
            }

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
                return BadRequest(result.Errors);
            }

            Console.WriteLine($"added successfully\n px: {user.Password}");
            user.Password = null;
            return Created("", user);
        }

        //POST
        //log in user
        [HttpPost("signinuser")]
        //[Route("api/user/signinuser")]
        public async Task<ActionResult<User>> SignInUser(User user)
        {
            Console.WriteLine("login user");
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            Console.WriteLine("a");


            Console.WriteLine($"username {user.UserName}\n password {user.Password}\n\n");

            var testuser = await _userManager.FindByNameAsync(user.UserName);
            var test2 = await _userManager.CheckPasswordAsync(
                testuser,
                user.Password
                );
            Console.WriteLine("\ncorrect password?");

            Console.WriteLine(test2.ToString());


            var testing = await _signInManager.CanSignInAsync(
                user: new IdentityUser(
                        userName: user.UserName
                    )
                );
            Console.WriteLine("\n har lov til å logge inn??");
            Console.WriteLine(testing.ToString());

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
                return BadRequest(result);

            }
            Console.WriteLine("\nok login!!");

            //sjekk om logget inn
            //var ferdig=await _userManager.is
            Console.WriteLine("er logget inn nå? ");
            //testuser.is
            //var ferdig=testuser.Identity.IsAuthenticated;
            //Console.WriteLine(ferdig);
            //bool val1 = HttpContext.Current.User.Identity.IsAuthenticated;

            //er dette ok?
            return Ok("user signed in");
        }

        /*
         bytte fra 1 til flere endpoint

            fungerer med registrering, men signInManager ødelegger!!!
            -url til controller
            -fetch url
            -setup proxy url
            -sign inn manager
         
         */

        // GET: api/Users/username
        //get userinfo from username
        [HttpGet("{username}")]
        //[Route("api/user/getuser")]


        //testing authorization/authentication
        [Authorize]

        public async Task<ActionResult<User>> GetUser(string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }

            return new User
            {
                UserName = user.UserName,
                Email = user.Email
            };
            //return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        }


        //kilde 2/2/23 https://www.endpointdev.com/blog/2022/06/implementing-authentication-in-asp.net-core-web-apis/
        // POST: api/Users/BearerToken
        [HttpPost("BearerToken")]
        public async Task<ActionResult<AuthenticationResponse>> CreateBearerToken(AuthenticationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Bad credentials");
            }

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                return BadRequest("Bad credentials");
            }

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid)
            {
                return BadRequest("Bad credentials");
            }

            var token = _jwtService.CreateToken(user);

            return Ok(token);

        }
    }
}
