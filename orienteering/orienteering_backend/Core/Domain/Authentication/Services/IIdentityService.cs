namespace orienteering_backend.Core.Domain.Authentication.Services
{

    public interface IIdentityService
    {
        public Task<bool> CreateUser(UserRegistration user);
        public Task<bool> SignInUser(UserSignIn user);
        public Task SignOutUser();
        public Guid? GetCurrentUserId();
    }
}
