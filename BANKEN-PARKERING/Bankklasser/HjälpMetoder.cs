using System.Text.Json;

namespace BANK
{
    public class HjälpMetoder
    {
        public DataBas dataBas;
        public List<Bankkonto> bankkonton { get; set; }

        public HjälpMetoder(DataBas dataBas)
        {
            this.dataBas = dataBas;
            bankkonton = dataBas.AllaBankontonFrånDB!;
        }

        public void Pausa()
        {
            Console.WriteLine("\n Tryck på en tangent för att komma till MENYN...");
            Console.ReadKey();
            Console.Clear();
        }

        public void SaveData(DataBas dataBas, string dataJSONfilPath = "BankData.json")
        {
            try
            {
                // Serialisera dataBas till JSON-sträng
                string updateradedataBas = JsonSerializer.Serialize(dataBas, new JsonSerializerOptions { WriteIndented = true });

                // Skriv JSON-strängen till filen
                File.WriteAllText(dataJSONfilPath, updateradedataBas);
                Console.WriteLine("Data har sparats korrekt.");
            }
            catch (Exception ex)
            {
                // Fångar och loggar eventuella fel vid filhantering
                Console.WriteLine($"Ett fel uppstod vid skrivning av filen: {ex.Message}");
            }
        }



    }
}