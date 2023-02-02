using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;


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

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        //public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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

            Console.WriteLine("added successfully");
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

            //var result = await _signInManager.PasswordSignInAsync(

            //    new IdentityUser()
            //    {
            //        UserName = user.UserName,

            //    },
            //    user.Password,
            //    false,
            //    false
            //);
            Console.WriteLine($"username {user.UserName}\n password {user.Password}\n\n");
            var result = await _signInManager.PasswordSignInAsync(

                user.UserName,
                user.Password,
                false,
                false
                
            );


            Console.WriteLine("har prøvd å logge inn\n");

            if (!result.Succeeded)
            {
                Console.WriteLine("problem!!\n");
                //Console.WriteLine(result.to);

                return BadRequest("problems logging in\n");
            }
            Console.WriteLine("ok login!!");

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


    }
}
