using System.ComponentModel.DataAnnotations;

namespace WebApp.Admin.Models
{
    public class AccessorDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

    }
}
