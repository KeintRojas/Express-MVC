using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace KFD.Models
{
    public class Dish
    {
        //Constructor
        public Dish(int id, string name, string description,
                    SqlMoney price, string picture, SqlBoolean isEnabled)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Picture = picture;
            IsEnabled = isEnabled;
        }
        public Dish()
        {
        }

        //Atributes
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public SqlMoney Price { get; set; }
        [Required]
        public string Picture { get; set; }
        public SqlBoolean IsEnabled { get; set; }

    }
}
