using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace orienteering_backend.Core.Domain.Authentication
{
    public interface IJwtService
    {

        public AuthenticationResponse CreateToken(IdentityUser user);
       // private JwtSecurityToken CreateJwtToken(Claim[] claims, SigningCredentials credentials, DateTime expiration);

    }
}
