using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace KFD.Models
{
    public class Dish
    {
        //Constructor
        public Dish(int id, string name, string description,
                    int price, string picture, int isEnabled)
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
        public int Price { get; set; }
        
        public string? Picture { get; set; }
        [Required]
        [Range(0,1)]
        public int IsEnabled { get; set; }

    }
}
