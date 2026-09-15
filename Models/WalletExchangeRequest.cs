namespace ExchangeRateHub.Models
{
    public class WalletExchangeRequest
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Amount { get; set; }
    }
}