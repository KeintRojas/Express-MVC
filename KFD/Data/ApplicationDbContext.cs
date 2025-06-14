using KFD.Models;
using Microsoft.EntityFrameworkCore;

namespace KFD.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) { 
        
        }
        public DbSet<Dish> dishes { get; set; }
        public DbSet<User> users { get; set; }
    }
}
