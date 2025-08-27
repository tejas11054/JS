using System.ComponentModel.DataAnnotations;

namespace TimeSheet.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required] public string ProjectName { get; set; }
        public string Description { get; set; }
    }
}
