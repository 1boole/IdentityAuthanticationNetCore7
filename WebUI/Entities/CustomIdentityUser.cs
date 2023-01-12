using Microsoft.AspNetCore.Identity;

namespace WebUI.Entities
{
    public class CustomIdentityUser : IdentityUser
    {
        public string? PhotoUrl { get; set; }
    }
}
