using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;

using LoginAPI.Entities;
using LoginAPI.Data;
using LoginAPI.Helpers;


namespace LoginAPI.Services
{
    public interface IUserService
    {
        Users Authenticate(string username, string password);
        IEnumerable<Users> GetAll();
        Users GetById(int id);
        Users Create(Users user, string password);
        void Update(Users user, string password = null);
        void Delete(int id);
    }
    public class UserService : IUserService
    {
        private LoginDbContext _context;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;

        public UserService(LoginDbContext context,
            IMapper mapper,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public Users Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users.SingleOrDefault(x => x.Email == username);

            // check if username exists
            if (user == null)
                return null;

            // check if password is correct
            if (user.Password == password)
                // authentication successful
                return user;
            else
                return null;

          
        }
        public IEnumerable<Users> GetAll()
        {
            return _context.Users;
        }

        public Users GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public Users Create(Users user, string password)
        {
            // validation
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Email == user.Email))
                throw new AppException("Email \"" + user.Email + "\" is already taken");


            _context.Users.Add(user);
            _context.SaveChanges();

            sendEmail(user);

            return user;
        }

        public void Update(Users userParam, string password = null)
        {
            var user = _context.Users.Find(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Email) && userParam.Email != user.Email)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Email == userParam.Email))
                    throw new AppException("Email " + userParam.Email + " is already taken");

                user.Email = userParam.Email;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                user.Password = password;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        private void sendEmail(Users user)
        {
            string message = string.Format(@"{0}", user.Email);
            
            _emailService.Send(
                to: user.Email,
                subject: "User Registration",
                html: $@"<h4>Registration</h4>
                         <p>Thanks for registering this email!</p>
                         {message}"
            );
        }

    }
}
