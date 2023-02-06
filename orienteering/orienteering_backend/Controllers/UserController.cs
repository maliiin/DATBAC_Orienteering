using Microsoft.AspNetCore.Http;
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
        //denne brukes ikke
        private readonly IJwtService _jwtService;
        private readonly IIdentityService _identityService;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtService jwtService, IIdentityService identityService)
        //public UserController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //brukes ikke??
            _jwtService = jwtService;
            _identityService = identityService;
        }

        //Get sign out
        //fix: bør dette være post heller? sender ikke inn noe data, og post gir 404
        [HttpGet]
        [Route("signOut")]
        public async Task SignOut()
        {
            Console.WriteLine("sign out");
            await _identityService.SignOutUser();
        }


        // POST: api/User
        [HttpPost("createuser")]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createduser = await _identityService.CreateUser(user);
            if (createduser == null) { return BadRequest("problem creating the user"); }

            return Created("", createduser);
        }

        //POST
        [HttpPost("signinuser")]
        public async Task<ActionResult<User>> SignInUser(User user)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userSignedIn = await _identityService.SignInUser(user);
            if (userSignedIn == null) { return BadRequest(new string("could not sign in the user")); }
            return Ok("user signed in");
        }


        [HttpGet]
        [Route("GetSignedInUserId")]
        public ActionResult<string> GetSignedInUserId()
        {
            
            //er en bruker logget in?
            var userIsAuthenticated = HttpContext.User.Identity.IsAuthenticated;

            //gir userid, men exeption om ingen er logget inn
            //var p = HttpContext.User.Claims.First().Value;

            //gir userid, men er null om ingen er logget inn (klikker hvis du kjører .value når ingen er logget inn (null.value))            
            var id = HttpContext.User.Claims.FirstOrDefault();
            Console.WriteLine($"is authenticated?? {userIsAuthenticated}");

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
        }


        // GET: api/User/username
        [HttpGet]
        //testing authorization/authentication
        [Authorize]
        [Route("{username}")]
        
        public async Task<ActionResult<User>> GetUser(string username)
        {
            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);

            //return CreatedAtAction("GetUser", new { username = user.UserName }, user);
        }


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
