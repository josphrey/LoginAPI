using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LoginAPI.Entities;
using Microsoft.Extensions.Configuration;

namespace LoginAPI.Data
{
    public class LoginDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public LoginDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to sql server database
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }
        public DbSet<Users> Users { get; set; }


    }
}
