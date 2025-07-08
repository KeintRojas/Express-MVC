using System.ComponentModel.DataAnnotations;

namespace KFD.Models
{
    public class Order
    {
        public Order()
        {
        }

        public Order(int id, int? clientID, string? description, int total, DateTime date, string state)
        {
            Id = id;
            this.clientID = clientID;
            Description = description;
            Total = total;
            Date = date;
            State = state;
        }

        public int Id { get; set; }
        public int? clientID { get; set; } 
        public string? Description { get; set; }
        [Required]
        public int Total {  get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string State { get; set; }
    }
}
