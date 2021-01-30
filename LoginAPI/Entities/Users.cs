using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace LoginAPI.Entities
{
    public class Users
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [EmailAddress]
        public string Password { get; set; }
    }
}
