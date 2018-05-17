using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectManager.Models
{
    public class ProjectViewModel
    {
        public Int64 Id { get; set; }

        [Required(ErrorMessage = "Please enter a name for the project.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter brief description for the project.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select a starting date for the project.")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Please select a ending date for the project.")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime EndDate { get; set; }
    }
}
