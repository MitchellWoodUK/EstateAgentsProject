using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace EstateAgents.Models
{
    public class CustomUserModel : IdentityUser
    {
        [Required]
        [MaxLength(100, ErrorMessage ="Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(11, ErrorMessage = "Telephone numbers cannot exceed 11 characters.")]
        public string Telephone { get; set; }
        [Required]
        public double Income { get; set; }
        [Required]
        [MaxLength(300, ErrorMessage = "Address cannot exceed 300 characters.")]
        public string Address { get; set; }
    }
}
