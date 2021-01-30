using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Model.Users
{
    public class UserModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
       
    }
}
