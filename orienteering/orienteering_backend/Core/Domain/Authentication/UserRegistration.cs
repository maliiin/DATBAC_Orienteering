using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Authentication
{
    public class UserRegistration
    {
        public UserRegistration(string userName, string password, string email)
        {
            UserName = userName;
            Password = password;
            Email = email;
        }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
    }
}
