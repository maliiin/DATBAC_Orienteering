using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;
using System.Threading.Tasks;
using orienteering_backend.Core.Domain.Login;


namespace orienteering_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;

       // public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
          //  _signInManager = signInManager;
        }

        //POST
        //log in user
        //[HttpPost]
        //public async Task<ActionResult<Microsoft.AspNetCore.Identity.SignInResult>> SignInUser(User user)
        //{
        //    Console.WriteLine("login user");
        //    if (!ModelState.IsValid) { return BadRequest(ModelState); }

        //    var result = await _signInManager.PasswordSignInAsync(

        //        new IdentityUser()
        //        {
        //            UserName = user.UserName,

        //        },
        //        user.Password,
        //        false,
        //        false
        //    );
        //    if (!result.Succeeded)
        //    {
        //        return BadRequest("problems logging in");
        //    }
        //    //er dette ok?
        //    return result;

        //}

        // POST: api/User
        //create user

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        //public string CreateUser()

        {
            Console.WriteLine("create user");
            if (!ModelState.IsValid)
            {
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
                return BadRequest(result.Errors);
            }

            user.Password = null;
            return Created("", user);
        }



        // GET: api/Users/username
        //get userinfo from username
        [HttpGet("{username}")]
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
