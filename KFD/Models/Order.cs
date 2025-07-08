using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KFD.Models
{
    public class Order
    {
        public Order()
        {
        }

        public Order(int id, string customerId, string? description, int total, DateTime date, string state)
        {
            Id = id;
            CustomerId = customerId;
            Description = description;
            Total = total;
            Date = date;
            State = state;
        }

        public int Id { get; set; }
        [Required]
        public string CustomerId { get; set; }
        public string? Description { get; set; }
        [Required]
        public int Total {  get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string State { get; set; }
    }
}
