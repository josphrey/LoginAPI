using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoginAPI.Entities;

namespace LoginAPI.Data
{
    public class DbInitializer
    {
        public static void Initialize(LoginDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var user = new Users[]
            {
                new Users() { FirstName ="Admin", LastName ="Admin" , Email = "admin@gmail.com" , Password = "P@ssvv0rd1234" },
                new Users() { FirstName ="Josephrey", LastName ="Salundaguit" , Email = "josphrey@gmail.com" , Password = "P@ssvv0rd1234" },
            };
            foreach (Users u in user)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();
        }
    }
}
