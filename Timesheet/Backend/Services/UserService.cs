using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TimeSheet.Models;
using TimeSheet.Repository;

namespace TimeSheet.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        public UserService(IUserRepository repo) => _repo = repo;

        public IEnumerable<User> GetAll() => _repo.GetAll();
        public User GetById(int id) => _repo.GetById(id);
        public User Add(User user) => _repo.Add(user);
        public User Update(User user) => _repo.Update(user);
        public void Delete(int id) => _repo.Delete(id);

        public LoginResponseViewModel Login(LoginViewModel usr)
        {
            var response = _repo.Login(usr);
            if (response.isSucess)
            {
                response.Token = GenerateToken(response.User);
            }
            return response;
        }

        private string GenerateToken(User user)
        {
            var config = new ConfigurationManager();
            config.AddJsonFile("appsettings.json");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["ConnectionStrings:secretkey"]));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email,user.Email),
                new Claim(ClaimTypes.Role,user.UserRole.Role.ToString())
            };
            var tokenOptions = new JwtSecurityToken(
                    issuer: "https://localhost:7069",
                    audience: "https://localhost:7069",
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(20),
                    signingCredentials: signingCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }
    }
}
