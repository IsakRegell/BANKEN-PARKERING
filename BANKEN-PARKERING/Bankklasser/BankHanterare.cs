using System.Text.Json;
using System.Transactions;

namespace BANK
{
    public class BankHanterare
    {
        public DataBas databas;
        public HjälpMetoder help;
        public Bankkonto bank;
        public List<Bankkonto> bankkonton { get; set; }
        public List<Transaction> transaction { get; set; }
        public BankHanterare(DataBas databas)
        {
            this.databas = databas;
            bankkonton = databas.AllaBankontonFrånDB!;
            transaction = databas.transactionList!;
            this.help = new HjälpMetoder(databas);
        }

        public void PrintTransaction()
        {
            foreach (var t in transaction)
            {
                Console.WriteLine(t);
            }

        }

        public Bankkonto? Pinkod()
        {
            Console.WriteLine("Skriv in din pinkod(4 siffror): ");
            int pinkodinput = Convert.ToInt32(Console.ReadLine());

            var konto = this.bankkonton.FirstOrDefault(b => b.Pinkod == pinkodinput);
            return konto;
        }

        public void TransferMoney()
        {
            Console.Write("Ange ditt kontonummer: ");
            int kontonummerFrom = Convert.ToInt32(Console.ReadLine());

            var userAccount = bankkonton.FirstOrDefault(k => k.Kontonummer == kontonummerFrom);
            if (userAccount == null)
            {
                Console.WriteLine("Kontot hittades inte");
                return;
            }

            Console.WriteLine("Vänligen ange din PIN-kod");
            int pinCode = Convert.ToInt32(Console.ReadLine());

            if (userAccount.Pinkod != pinCode)
            {
                Console.WriteLine("Felaktig PIN-kod. Transaktionen avbryts.");
                return;
            }

            Console.WriteLine("Hej! Skriv in beloppet du vill föraöver : ");
            int moneyToTransfer = Convert.ToInt32(Console.ReadLine());

            if (userAccount.Saldo < moneyToTransfer)
            {
                Console.WriteLine("Otillräckligt saldo...");
                return;
            }

            Console.WriteLine("Vänligen ange konto nummret där pengarna ska sättas in : ");
            int kontonummerToTransfer = Convert.ToInt32(Console.ReadLine());

            var targetAccount = bankkonton.FirstOrDefault(k => k.Kontonummer == kontonummerToTransfer);
            if (targetAccount != null)
            {
                Console.WriteLine($"Kontot hittades, ägaren av kontot är *{targetAccount.Namn}*");

                // Uppdatera saldon
                userAccount.Saldo -= moneyToTransfer;
                targetAccount.Saldo += moneyToTransfer;

                Console.WriteLine($"Överföring genomförd. Ditt nya saldo: {userAccount.Saldo}.");
                Console.WriteLine($"Mottagarens nya saldo: {targetAccount.Saldo}.");

                help.SaveData(databas);
                LogTransaction(userAccount.Kontonummer, targetAccount.Kontonummer, moneyToTransfer);
            }
            else
            {
                Console.WriteLine("Mottagarkontot hittades inte");
            }

        }

        public void EditAccountName()
        {
            Console.WriteLine("Ange pinkoden på kontot du vill uppdatera");
            int updKonto = Convert.ToInt32(Console.ReadLine());

            var EditAcc = bankkonton.FirstOrDefault(k => k.Pinkod == updKonto);
            if (EditAcc != null)
            {
                try
                {
                    Console.WriteLine("Ange nytt namn till kontot : ");
                    string newUPDname = Console.ReadLine()!;
                    Console.WriteLine($"Namnet har uppdaterats från *{EditAcc.Namn}* till *{newUPDname}*");
                    EditAcc.Namn = newUPDname;
                }
                catch
                {
                    Console.WriteLine("Hittade inget konto som matcha pinkoden");
                }
            }
            else
            {
                Console.WriteLine("Hittade inget konto!");
            }
        }

        public void LogTransaction(int fromAccount, int toAccount, int amount)
        {
            var transaction = new Transaction
            {
                FromAccount = fromAccount,
                ToAccount = toAccount,
                Amount = amount,
                Date = DateTime.Now
            };

            string logFilePath = "BankData.json";

            // Check if the file exists
            if (File.Exists(logFilePath))
            {
                // Read existing data from the file
                string existingData = File.ReadAllText(logFilePath);

                // Deserialize the existing data into DataBas object
                var existingBankData = JsonSerializer.Deserialize<DataBas>(existingData) ?? new DataBas();

                // Initialize the transactionList if it's null
                if (existingBankData.transactionList == null)
                {
                    existingBankData.transactionList = new List<Transaction>();
                }

                // Add the new transaction to the transaction list
                existingBankData.transactionList.Add(transaction);

                // Save the updated data back to the JSON file, including both Bankkonton and transactionList
                help.SaveData(existingBankData);
            }
            else
            {
                Console.WriteLine("Ett fel uppstod...");
            }

        }
    }
}