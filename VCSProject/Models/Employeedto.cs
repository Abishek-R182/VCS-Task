using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace VCSProject.Models
{
    public class Employeedto
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
        public IFormFile Image { get; set; }
    }
}
