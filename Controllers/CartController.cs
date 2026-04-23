using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonCardShop.Data;
using PokemonCardShop.Models;
using System.Text.Json;

namespace PokemonCardShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const string CartKey = "Cart";

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult AddToCart(int id)
        {
            var product = _context.PokemonCards.FirstOrDefault(x => x.Id == id);
            if (product == null)
                return NotFound();

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.PokemonCardId == id);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItem
                {
                    PokemonCardId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(x => x.PokemonCardId == id);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public IActionResult Checkout()
        {
            var cart = GetCart();

            if (!cart.Any())
                return RedirectToAction(nameof(Index));

            var order = new Order();

            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                order.Email = User.Identity.Name ?? string.Empty;
            }

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            var cart = GetCart();

            if (!cart.Any())
            {
                ModelState.AddModelError("", "Корзина пуста");
            }

            if (User.Identity != null && User.Identity.IsAuthenticated && string.IsNullOrWhiteSpace(order.Email))
            {
                order.Email = User.Identity.Name ?? string.Empty;
            }

            if (!ModelState.IsValid)
            {
                return View(order);
            }

            order.OrderDate = DateTime.UtcNow;
            order.TotalAmount = cart.Sum(x => x.Price * x.Quantity);

            foreach (var item in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    PokemonCardId = item.PokemonCardId,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove(CartKey);

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Orders()
        {
            var orders = _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        private List<CartItem> GetCart()
        {
            var cartJson = HttpContext.Session.GetString(CartKey);

            if (string.IsNullOrEmpty(cartJson))
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        private void SaveCart(List<CartItem> cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            HttpContext.Session.SetString(CartKey, cartJson);
        }
    }
}