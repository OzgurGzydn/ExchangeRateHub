using Microsoft.AspNetCore.Mvc;
using ExchangeRateHub.Services;
using System.Text.Json;

namespace ExchangeRateHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        private readonly ExchangeService _exchangeService;

        public ExchangeController(ExchangeService exchangeService)
        {
            _exchangeService = exchangeService;
        }

        [HttpGet("rates")]
        public async Task<IActionResult> GetRates()
        {
            try
            {
                var rates = await _exchangeService.GetRatesAsync();
                return Ok(rates);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve exchange rates.");
            }
        }

        [HttpGet("nbp-rates")]
        public async Task<IActionResult> GetNbpRates()
        {
            try
            {
                var rates = await _exchangeService.GetNbpRatesAsync();
                return Ok(rates);
            }
            catch (Exception)
            {
                return StatusCode(500, "Failed to retrieve NBP exchange rates.");
            }
        }

        [HttpGet("convert")]
        public async Task<IActionResult> Convert(string from, string to, double amount)
        {
            if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
                return BadRequest("From and To currency codes are required.");

            if (amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            try
            {
                var url = $"https://api.frankfurter.app/latest?amount={amount}&from={from.ToUpper()}&to={to.ToUpper()}";

                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return BadRequest("Conversion failed. Please check currency codes.");

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<object>(json);

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(500, "An unexpected error occurred during conversion.");
            }
        }
    }
}