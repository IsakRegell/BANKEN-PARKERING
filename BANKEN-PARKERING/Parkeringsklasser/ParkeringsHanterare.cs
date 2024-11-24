using System;
using System.Text.RegularExpressions;

namespace BANK
{
    public class ParkeringsHanterare
    {
        public DataBas databas;
        public HjälpMetoder help;
        public Bankkonto bankkonton;
        public BankHanterare bankH;
        public Parkering parkeringsklass;

        public List<Bankkonto> bankkonto { get; set; }
        public List<Transaction> transaction { get; set; }
        public List<Parkering> AktivParkering { get; set; }
        public List<Parkering> AvslutadParkering { get; set; }

        public ParkeringsHanterare(DataBas databas)
        {
            this.databas = databas;
            bankkonto = databas.AllaBankontonFrånDB!;
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


        public void AvslutaParkering()
        {
            if (AktivParkering == null || AktivParkering.Count == 0)
            {
                Console.WriteLine("Inga aktiva parkeringar är tillgängliga.");
                return;
            }

            Console.WriteLine("Ange registreringsnummer för att avsluta parkering:");
            string regNummer = Console.ReadLine()!.ToUpper();
            var parkering = databas.AktivaParkeringarFrånDB!.FirstOrDefault(p => p.RegNummer == regNummer);

            if (parkering != null)
            {
                decimal kostnad = parkering.GetKostnad();

                // Låt användaren välja ett bankkonto
                Console.WriteLine("Välj bankkonto att debitera från:");
                int index = 1;
                foreach (var konto in bankkonto)
                {
                    Console.WriteLine($"{index}. Kontonummer: {konto.Kontonummer}, Saldo: {konto.Saldo}kr");
                    index++;
                }

                int kontoVal = 0;
                bool validChoice = false;

                while (!validChoice)
                {
                    Console.Write("Ange siffra för valt konto: ");
                    string input = Console.ReadLine()!;

                    if (int.TryParse(input, out kontoVal) && kontoVal > 0 && kontoVal <= bankkonto.Count)
                    {
                        validChoice = true;
                    }
                    else
                    {
                        Console.WriteLine("Ogiltigt val. Försök igen.");
                    }
                }

                var valtKonto = bankkonto[kontoVal - 1];

                // Kontrollera saldo och debitera
                if (valtKonto.Saldo >= (int)Math.Ceiling(kostnad))
                {
                    valtKonto.Saldo -= (int)Math.Ceiling(kostnad);

                    // Uppdatera listorna
                    databas.AktivaParkeringarFrånDB!.Remove(parkering);
                    databas.AvslutadeParkeringarFrånDB!.Add(parkering);

                    // Skapa och spara transaktion
                    var transaktion = new Transaction
                    {
                        FromAccount = valtKonto.Kontonummer,
                        ToAccount = 999901, // Exempel på företagskonto
                        Amount = (int)Math.Ceiling(kostnad),
                        Date = DateTime.Now
                    };

                    databas.transactionList!.Add(transaktion);
                    Console.WriteLine($"Parkeringen avslutad. Totalkostnad: {(int)Math.Ceiling(kostnad)} kr");
                }
                else
                {
                    Console.WriteLine("Det valda bankkontot har inte tillräckligt med saldo för att avsluta parkeringen.");
                }
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