using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TimeSheet.Data;
using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) => _context = context;

        public IEnumerable<User> GetAll() => _context.Users.ToList();

        public User GetById(int id) => _context.Users.Find(id);

        public User Add(User user)
        {
            // Hash the password before saving
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public User Update(User user)
        {
            // If password is being updated, hash it again
            if (!string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            _context.Users.Update(user);
            _context.SaveChanges();
            return user;
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

        public LoginResponseViewModel Login(LoginViewModel usr)
        {
            // Get user by username only
            var user = _context.Users
                .Include(u => u.UserRole)
                .FirstOrDefault(u => u.Name.Equals(usr.UserName));

            LoginResponseViewModel response;

            // ✅ Verify hashed password
            if (user != null && BCrypt.Net.BCrypt.Verify(usr.Password, user.Password))
            {
                response = new LoginResponseViewModel
                {
                    isSucess = true,
                    User = user,
                    Token = "" // you can set JWT or leave empty
                };
                return response;
            }

            response = new LoginResponseViewModel
            {
                isSucess = false,
                User = null,
                Token = ""
            };
            return response;
        }
    }
}
