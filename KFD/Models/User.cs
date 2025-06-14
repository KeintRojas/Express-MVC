using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Runtime.InteropServices;

namespace KFD.Models
{
    public class User
    {
        //Constructor
        protected User(int id, string name, string password, string rol,
                       string email, string address, int isEnabled)
        {
            Id = id;
            Name = name;
            Password = password;
            Rol = rol;
            Email = email;
            Address = address;
            IsEnabled = isEnabled;
        }

        public User()
        {
        }

        //Atributes
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [PasswordPropertyText]
        [Length(8,12)]
        public string Password { get; set; }
        [Required]
        public string Rol {  get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string Address { get; set; }
        [Required]
        [Range(0, 1)]
        public int IsEnabled { get; set; }
        
    }
}
