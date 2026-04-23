using PokemonCardShop.Models;
using System.Text.Json;

namespace PokemonCardShop.Helpers
{
    public static class CartHelper
    {
        public static int GetCartCount(HttpContext context)
        {
            var cartJson = context.Session.GetString("Cart");

            if (string.IsNullOrEmpty(cartJson))
                return 0;

            var cart = JsonSerializer.Deserialize<List<CartItem>>(cartJson);

            return cart?.Sum(x => x.Quantity) ?? 0;
        }
    }
}