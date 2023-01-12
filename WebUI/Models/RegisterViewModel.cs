using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="This is required because wanted to my taste")]
        public IFormFile PhotoUpload { get; set; }
    }
}
