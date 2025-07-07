using Microsoft.AspNetCore.Identity;

namespace KFD.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public int IsEnabled { get; set; }

        public string Address { get; set; }
    }
}
