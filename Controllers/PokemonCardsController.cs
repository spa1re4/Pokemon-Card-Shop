using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokemonCardShop.Data;
using PokemonCardShop.Models;

namespace PokemonCardShop.Controllers
{
    public class PokemonCardsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PokemonCardsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Доступно всем
        public async Task<IActionResult> Index()
        {
            var cards = await _context.PokemonCards.ToListAsync();
            return View(cards);
        }

        // Доступно всем
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.PokemonCards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
                return NotFound();

            return View(card);
        }

        // Только админ
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // Только админ
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(PokemonCard pokemonCard)
        {
            if (!ModelState.IsValid)
                return View(pokemonCard);

            _context.PokemonCards.Add(pokemonCard);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Только админ
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.PokemonCards.FindAsync(id);

            if (card == null)
                return NotFound();

            return View(card);
        }

        // Только админ
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, PokemonCard pokemonCard)
        {
            if (id != pokemonCard.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(pokemonCard);

            try
            {
                _context.PokemonCards.Update(pokemonCard);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = await _context.PokemonCards.AnyAsync(e => e.Id == pokemonCard.Id);
                if (!exists)
                    return NotFound();

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // Только админ
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var card = await _context.PokemonCards.FirstOrDefaultAsync(x => x.Id == id);

            if (card == null)
                return NotFound();

            return View(card);
        }

        // Только админ
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var card = await _context.PokemonCards.FindAsync(id);

            if (card != null)
            {
                _context.PokemonCards.Remove(card);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}