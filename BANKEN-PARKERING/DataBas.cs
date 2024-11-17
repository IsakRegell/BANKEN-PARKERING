using System.Text.Json.Serialization;
using System.Transactions;

namespace BANK
{
    public class DataBas
    {
        [JsonPropertyName("Bankkonton")]
        public List<Bankkonto>? AllaBankontonFrånDB { get; set; }

        [JsonPropertyName("transactionList")]
        public List<Transaction>? transactionList { get; set; }

        [JsonPropertyName("AktivaParkeringar")]
        public List<Parkering>? AktivaParkeringarFrånDB { get; set; } = new List<Parkering>();

        [JsonPropertyName("AvslutadeParkeringar")]
        public List<Parkering>? AvslutadeParkeringarFrånDB { get; set; } = new List<Parkering>();

    }

}