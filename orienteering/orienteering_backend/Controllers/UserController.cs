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

        private readonly IIdentityService _identityService;

        public UserController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        [Route("signOut")]
        public async Task<ActionResult> SignOut()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            await _identityService.SignOutUser();
            return Ok();
        }


        [HttpPost("createuser")]
        public async Task<ActionResult<UserRegistration>> CreateUser(UserRegistration user)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var createduser = await _identityService.CreateUser(user);
            if (createduser == null) { return BadRequest("problem creating the user"); }

            return Created("", createduser);
        }

        [HttpPost("signinuser")]
        public async Task<ActionResult<UserSignIn>> SignInUser(UserSignIn user)
        {

            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userSignedIn = await _identityService.SignInUser(user);
            if (userSignedIn == null) { return BadRequest(new string("could not sign in the user")); }
            return Ok("user signed in");
        }


        [HttpGet]
        [Route("GetSignedInUserId")]
        public ActionResult<bool> GetSignedInUserId()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var id = _identityService.GetCurrentUserId();

            if (id is null)
            {
                return false;

            }
            return true;
        }
    }
}
