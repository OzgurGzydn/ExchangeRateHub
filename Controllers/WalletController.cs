using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExchangeRateHub.Models;
using System.Text.Json;

namespace ExchangeRateHub.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private static List<WalletBalance> balances = new List<WalletBalance>();
        private static List<string> transactionHistory = new List<string>();

        [HttpGet]
        public IActionResult GetWallet()
        {
            return Ok(balances);
        }

        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            return Ok(transactionHistory);
        }

        [HttpPost("deposit")]
        public IActionResult Deposit(WalletTransaction transaction)
        {
            if (transaction == null || string.IsNullOrWhiteSpace(transaction.Currency))
                return BadRequest("Currency is required.");

            if (transaction.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var currency = transaction.Currency.ToUpper();

            var existingBalance = balances.FirstOrDefault(x => x.Currency == currency);

            if (existingBalance == null)
            {
                balances.Add(new WalletBalance
                {
                    Currency = currency,
                    Amount = transaction.Amount
                });
            }
            else
            {
                existingBalance.Amount += transaction.Amount;
            }

            transactionHistory.Add($"+{transaction.Amount} {currency} Deposit");

            return Ok(balances);
        }

        [HttpPost("exchange")]
        public async Task<IActionResult> Exchange(WalletExchangeRequest request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.FromCurrency) ||
                string.IsNullOrWhiteSpace(request.ToCurrency))
            {
                return BadRequest("FromCurrency and ToCurrency are required.");
            }

            if (request.Amount <= 0)
                return BadRequest("Amount must be greater than zero.");

            var from = request.FromCurrency.ToUpper();
            var to = request.ToCurrency.ToUpper();

            var fromBalance = balances.FirstOrDefault(x => x.Currency == from);

            if (fromBalance == null || fromBalance.Amount < request.Amount)
                return BadRequest("Insufficient balance.");

            var url = $"https://api.frankfurter.app/latest?amount={request.Amount}&from={from}&to={to}";

            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return BadRequest("Exchange failed. Please check currency codes.");

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);

            var convertedAmount = document.RootElement
                .GetProperty("rates")
                .GetProperty(to)
                .GetDouble();

            fromBalance.Amount -= request.Amount;

            var toBalance = balances.FirstOrDefault(x => x.Currency == to);

            if (toBalance == null)
            {
                balances.Add(new WalletBalance
                {
                    Currency = to,
                    Amount = convertedAmount
                });
            }
            else
            {
                toBalance.Amount += convertedAmount;
            }

            transactionHistory.Add($"{request.Amount} {from} exchanged to {convertedAmount} {to}");

            return Ok(new
            {
                Message = "Exchange completed successfully.",
                FromCurrency = from,
                ToCurrency = to,
                OriginalAmount = request.Amount,
                ConvertedAmount = convertedAmount,
                Wallet = balances,
                History = transactionHistory
            });
        }
    }
}