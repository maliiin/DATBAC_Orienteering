using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Login;

namespace orienteering_backend.Core.Domain.Authentication.Services
{

    public interface IIdentityService
    {
        public Task<User> CreateUser(User user);
        public Task<User> SignInUser(User user);

        public Task SignOutUser();


    }
}
