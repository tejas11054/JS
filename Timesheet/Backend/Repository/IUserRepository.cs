using TimeSheet.Models;

namespace TimeSheet.Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Add(User user);
        User Update(User user);
        void Delete(int id);

        public LoginResponseViewModel Login(LoginViewModel usr);
        // User Login(string email, string password);
    }
}
