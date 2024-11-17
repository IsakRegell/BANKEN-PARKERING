using System;
using System.Text.RegularExpressions;

namespace BANK
{
    public class ParkeringsHanterare
    {
        public DataBas databas;
        public HjälpMetoder help;
        public Bankkonto bank;
        public BankHanterare bankH;
        public Parkering parkeringsklass;

        public List<Bankkonto> bankkonton { get; set; }
        public List<Transaction> transaction { get; set; }
        public List<Parkering> AktivParkering { get; set; }
        public List<Parkering> AvslutadParkering { get; set; }

        public ParkeringsHanterare(DataBas databas)
        {
            this.databas = databas;
            bankkonton = databas.AllaBankontonFrånDB!;
            transaction = databas.transactionList!;
            AktivParkering = databas.AktivaParkeringarFrånDB!;
            AvslutadParkering = databas.AvslutadeParkeringarFrånDB!;
            this.help = new HjälpMetoder(databas);
            this.bankH = new BankHanterare(databas);
            this.parkeringsklass = new Parkering("");
        }

        public void StartaParkering()
        {
            //Ny teknik som väljer mönster som strängen måste följa.
            string pattern = @"^[A-Za-zåäöÅÄÖ]{3}[0-9]{3}$";

            string? regnmrParkering = null;
            bool isValid = false;

            while (!isValid)
            {
                Console.WriteLine("Ange regNummer ex(ABC123) på bilen : ");
                regnmrParkering = Console.ReadLine()!.ToUpper();

                if (string.IsNullOrEmpty(regnmrParkering))
                {
                    Console.WriteLine("Regnummer kan inte vara tom. Försök igen.");
                    continue;
                }

                regnmrParkering = regnmrParkering.ToUpper();

                if (Regex.IsMatch(regnmrParkering, pattern))
                {
                    isValid = true;
                }
                else
                {
                    Console.WriteLine("Felaktigt format! Regnummer måste bestå av 3 bokstäver följt av 3 siffror, exempel: ABC123.");
                }
            }

            var Addedparkering = new Parkering(regnmrParkering!);
            databas.AktivaParkeringarFrånDB!.Add(Addedparkering);
            Console.WriteLine($"Din parkering har startat! Kostnaden är {parkeringsklass.KostnadPerSekund}kr/s");

            help.SaveData(databas);
            help.Pausa();
        }

        public void AvslutaParkering(Bankkonto konto)
        {
            Console.WriteLine("Ange registreringsnummer för att avsluta parkering:");
            string regNummer = Console.ReadLine()!.ToUpper();
            var parkering = databas.AktivaParkeringarFrånDB!.FirstOrDefault(p => p.RegNummer == regNummer);

            if (parkering != null)
            {
                decimal kostnad = parkering.GetKostnad();
                databas.AktivaParkeringarFrånDB!.Remove(parkering);
                databas.AvslutadeParkeringarFrånDB!.Add(parkering);

                var transaktion = new Transaction
                {
                    FromAccount = konto.Kontonummer,
                    ToAccount = 999901,
                    Amount = kostnad,
                    Date = DateTime.Now
                };

                databas.transactionList!.Add(transaktion);
                Console.WriteLine($"Parkeringen avslutad. Totalkostnad: {kostnad} kr");
            }
            else
            {
                Console.WriteLine("Parkeringen hittades inte.");
            }
            help.SaveData(databas);
            help.Pausa();
        }

        public void VisaAktivaParkeringar()
        {
            foreach (var parkering in databas.AktivaParkeringarFrånDB!)
            {
                decimal kostnad = parkering.GetKostnad();
                Console.WriteLine($"RegNr: {parkering.RegNummer}, Starttid: {parkering.StartTid}, Kostnad: {kostnad} kr");
            }
            help.Pausa();
        }
    }
}