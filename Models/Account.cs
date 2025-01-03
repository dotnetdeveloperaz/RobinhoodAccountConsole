using System.Text.Json.Serialization;

namespace RobinhoodAccountConsole.Models
{
    public class Account
    {
        [JsonPropertyName("account_number")]
        public string? AccountNumber { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }

        [JsonPropertyName("buying_power")]
        public string? BuyingPower { get; set; }

        [JsonPropertyName("buying_power_currency")]
        public string? BuyingPowerCurrency { get; set; }
    }
}
