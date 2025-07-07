using System.ComponentModel.DataAnnotations;

namespace KFD.Models
{
    public class Order
    {
        public Order()
        {
        }

        public Order(int id, string? description, int total, DateTime date, string state)
        {
            Id = id;
            Description = description;
            Total = total;
            Date = date;
            State = state;
        }

        public int Id { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Total {  get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string State { get; set; }
    }
}
