using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TimeSheet.Models
{
    public class Timesheet
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public User? User { get; set; }   
        [Required]
        public int ProjectId { get; set; }
        public Project? Project { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int HoursWorked { get; set; }

        public string? TaskDescription { get; set; }
    }
}
