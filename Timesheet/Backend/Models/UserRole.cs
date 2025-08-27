using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public enum Role { ADMIN, EMPLOYEE }
    public class UserRole
    {
        [Key]
        public int RoleID { get; set; }
        [Required(ErrorMessage = "Role is Required!")]
        public Role Role { get; set; }
        public virtual IEnumerable<User>? Users { get; set; }
    }
}
