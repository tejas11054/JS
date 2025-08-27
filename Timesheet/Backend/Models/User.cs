using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace TimeSheet.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required] 
        public string Name { get; set; }
        [Required, EmailAddress] 
        public string Email { get; set; }
        [Required] 
        public string Password { get; set; }

        [ForeignKey("UserRole")]
        public int? RoleId { get; set; }
        public virtual UserRole? UserRole { get; set; }
    }
}
