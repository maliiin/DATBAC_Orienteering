using System.ComponentModel.DataAnnotations;

//kilde 2/2/23: https://www.endpointdev.com/blog/2022/06/implementing-authentication-in-asp.net-core-web-apis/

namespace orienteering_backend.Core.Domain.Authentication
{
    public class AuthenticationRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
