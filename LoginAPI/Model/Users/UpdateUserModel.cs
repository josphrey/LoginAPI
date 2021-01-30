using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Model.Users
{
    public class UpdateUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
