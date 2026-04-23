using System.ComponentModel.DataAnnotations;

namespace PokemonCardShop.Models
{
    public class PokemonCard
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Series { get; set; } = string.Empty;

        public string Rarity { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string? ImageUrl { get; set; }

        public string? Description { get; set; }
    }
}