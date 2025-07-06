using KFD.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KFD.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { 
        
        }
        public DbSet<Dish> dishes { get; set; }
        public DbSet<User> users { get; set; }
        //public DbSet<ApplicationUser> AppUsers { get; set; }
    }
}
