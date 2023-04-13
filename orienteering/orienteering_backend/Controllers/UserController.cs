using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Authentication;
using orienteering_backend.Core.Domain.Authentication.Services;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        //denne brukes ikke
        private readonly IIdentityService _identityService;

        public UserController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IIdentityService identityService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _identityService = identityService;
        }

        //Get sign out
        //fix: bør dette være post heller? sender ikke inn noe data, og post gir 404
        [HttpGet]
        [Route("signOut")]
        public async Task SignOut()
        {
            await _identityService.SignOutUser();
        }


        // POST: api/User
        [HttpPost("createuser")]
        public async Task<ActionResult<UserRegistration>> CreateUser(UserRegistration user)
        {
            //fix-hva gjør modelstate? bør dette være flere steder??
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
        public async Task<ActionResult<UserSignIn>> SignInUser(UserSignIn user)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userSignedIn = await _identityService.SignInUser(user);
            if (userSignedIn == null) { return BadRequest(new string("could not sign in the user")); }
            return Ok("user signed in");
        }


        //fiks-er denne nødvendig å ha? nå kan dette skje i en service heller
        [HttpGet]
        [Route("GetSignedInUserId")]
        public ActionResult<object> GetSignedInUserId()
        {
            var id = HttpContext.User.Claims.FirstOrDefault();

            if (id is null) 
            { 
                return NotFound();

            }
            else             
            {
                //fiks fix returtypen her, bør lage eget objekt, ikke sende identity user!!
                var user1 = new IdentityUser();
                user1.Id = id.Value;
                return user1;
            }
        }
    }
}
