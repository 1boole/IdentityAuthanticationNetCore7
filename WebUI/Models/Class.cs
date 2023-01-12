using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace WebUI.Models
{
    public class ProfileViewModel
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
