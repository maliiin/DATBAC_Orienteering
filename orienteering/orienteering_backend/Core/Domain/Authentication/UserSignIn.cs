using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Authentication
{
    public class UserSignIn
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
