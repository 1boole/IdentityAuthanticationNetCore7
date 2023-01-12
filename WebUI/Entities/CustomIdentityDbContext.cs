using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace WebUI.Entities
{

    public class CustomIdentityDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole, string>
    {
        public CustomIdentityDbContext(DbContextOptions<CustomIdentityDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

    }

}
