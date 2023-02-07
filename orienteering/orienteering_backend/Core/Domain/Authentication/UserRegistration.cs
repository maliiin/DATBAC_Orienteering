using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Authentication
{
    public class UserRegistration
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
