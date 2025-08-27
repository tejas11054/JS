namespace TimeSheet.Models
{
    public class LoginResponseViewModel
    {
        public bool isSucess { get; set; }
        public User? User { get; set; }
        public string Token { get; set; }
    }
}
