using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Model.Users
{
    public class AuthenticateModel
    {
        [Required]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
