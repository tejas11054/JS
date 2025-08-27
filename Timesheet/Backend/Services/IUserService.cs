using TimeSheet.Models;

namespace TimeSheet.Services
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Add(User user);
        User Update(User user);
        void Delete(int id);

        public LoginResponseViewModel Login(LoginViewModel usr);
    }
}
