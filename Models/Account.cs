using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RobinhoodCreateSignature.Models
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
