using Microsoft.AspNetCore.Mvc;

namespace orienteering_backend.Core.Domain.Authentication.Services
{

    public interface IIdentityService
    {
        public Task<UserRegistration> CreateUser(UserRegistration user);
        public Task<UserSignIn> SignInUser(UserSignIn user);
        public Task SignOutUser();
        public Guid? GetCurrentUserId();
    }
}
