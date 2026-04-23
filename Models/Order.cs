using System.ComponentModel.DataAnnotations;

namespace PokemonCardShop.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите имя")]
        public string CustomerName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Введите адрес")]
        public string Address { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}