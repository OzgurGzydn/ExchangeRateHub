using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExchangeRateHub.Models;

namespace ExchangeRateHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritesController : ControllerBase
    {
        private static List<FavoriteCurrency> favorites = new List<FavoriteCurrency>();

        [HttpGet]
        public IActionResult GetFavorites()
        {
            return Ok(favorites);
        }

        [HttpPost]
        public IActionResult AddFavorite(FavoriteCurrency favorite)
        {
            if (favorite == null || string.IsNullOrWhiteSpace(favorite.Currency))
                return BadRequest("Currency is required.");

            favorite.Id = favorites.Count + 1;
            favorite.Currency = favorite.Currency.ToUpper();
            favorites.Add(favorite);

            return Ok(favorite);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateFavorite(int id, FavoriteCurrency favorite)
        {
            if (favorite == null || string.IsNullOrWhiteSpace(favorite.Currency))
                return BadRequest("Currency is required.");

            var existing = favorites.FirstOrDefault(x => x.Id == id);

            if (existing == null)
                return NotFound("Favorite currency not found.");

            existing.Currency = favorite.Currency.ToUpper();

            return Ok(existing);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteFavorite(int id)
        {
            var favorite = favorites.FirstOrDefault(x => x.Id == id);

            if (favorite == null)
                return NotFound("Favorite currency not found.");

            favorites.Remove(favorite);

            return Ok("Favorite currency deleted successfully.");
        }
    }
}