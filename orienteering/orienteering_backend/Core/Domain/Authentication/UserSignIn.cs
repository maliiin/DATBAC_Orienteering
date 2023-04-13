using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Authentication
{
    public class UserSignIn
    {
        public UserSignIn(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }

        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
