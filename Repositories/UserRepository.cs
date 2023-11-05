using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PingChecker.Data;
using PingChecker.Models;
using PingChecker.Services;

namespace PingChecker.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;
        private readonly JwtTokenService _jwtService;

        public UserRepository(DataContext dataContext, JwtTokenService jwtTokenService)
        {
            _dataContext = dataContext;
            _jwtService = jwtTokenService;
        }
        public User FindUserByUsername(string username)
        {

            return _dataContext.Users.Where(u => u.FirstName == username).FirstOrDefault();
        }

        public User GetUser(long ID)
        {
            throw new NotImplementedException();
        }

        public User GetUserByUsername(string userName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetUsers()
        {
            throw new NotImplementedException();
        }

        public void RegisterUser(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;
            UpdateUserToken(user);
            // Console.WriteLine()
            _dataContext.Add(user);
        }

        public bool SaveChanges()
        {
            return _dataContext.SaveChanges() >= 0;
        }

        public void UpdateUserToken(User user)
        {
            string token = _jwtService.GenerateJwtToken(user.ID.ToString());
            user.TokenExpirationTime = DateTime.UtcNow.AddHours(1);
            user.Token = token;
        }
    }
}