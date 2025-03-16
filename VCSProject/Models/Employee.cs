using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VCSProject.Models
{
    public class Employee
    {
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Qualification { get; set; }

        [NotMapped]
        public string[] Qualifications { get; set; }

        public string? ImagePath { get; set; }

        [NotMapped]
        public IFormFile Image {  get; set; }
    }
}
