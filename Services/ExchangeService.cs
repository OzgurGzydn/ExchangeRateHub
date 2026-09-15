using ExchangeRateHub.Models;
using System.Text.Json;

namespace ExchangeRateHub.Services
{
    public class ExchangeService
    {
        private readonly HttpClient _httpClient;

        public ExchangeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CurrencyRate>> GetRatesAsync()
        {
            var url = "https://api.frankfurter.app/latest?from=EUR";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("External API request failed.");

            var json = await response.Content.ReadAsStringAsync();

            using var document = JsonDocument.Parse(json);
            var ratesElement = document.RootElement.GetProperty("rates");

            var rates = new List<CurrencyRate>();

            rates.Add(new CurrencyRate
            {
                Currency = "EUR",
                Rate = 1.0
            });

            foreach (var item in ratesElement.EnumerateObject())
            {
                rates.Add(new CurrencyRate
                {
                    Currency = item.Name,
                    Rate = item.Value.GetDouble()
                });
            }

            return rates;
        }

        public async Task<List<NbpRate>> GetNbpRatesAsync()
        {
            var url = "https://api.nbp.pl/api/exchangerates/tables/a/?format=json";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                throw new Exception("NBP API request failed.");

            var json = await response.Content.ReadAsStringAsync();
            using var document = JsonDocument.Parse(json);

            var ratesArray = document.RootElement[0].GetProperty("rates");
            var result = new List<NbpRate>();

            foreach (var item in ratesArray.EnumerateArray())
            {
                result.Add(new NbpRate
                {
                    Currency = item.GetProperty("currency").GetString(),
                    Code = item.GetProperty("code").GetString(),
                    Mid = item.GetProperty("mid").GetDouble()
                });
            }

            return result;
        }
    }
}