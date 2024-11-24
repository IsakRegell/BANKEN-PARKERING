using System.Text.Json;
using Spectre.Console;
using Figgle;

namespace BANK
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dataJSONfilPath = "BankData.json"; // Ange sökvägen till JSON-filen

            string allaDataSomJSONType = File.ReadAllText(dataJSONfilPath); // Läs JSON-innehållet från filen

            DataBas databas = JsonSerializer.Deserialize<DataBas>(allaDataSomJSONType)!; // Deserialisera JSON-innehållet till ett DataBas-objekt

            BankHanterare bankHanterare = new BankHanterare(databas);
            HjälpMetoder help = new HjälpMetoder(databas);
            ParkeringsHanterare Parkering = new ParkeringsHanterare(databas);

            bool ProgramIsRuning = true;

            // Visa startlogga med Figgle
            Console.Clear();
            AnsiConsole.Markup("[bold yellow]Välkommen till:[/]");
            Console.WriteLine();
            Console.WriteLine(FiggleFonts.Standard.Render("BANKAPP"));

            

            while (ProgramIsRuning)
            {

                var AccChoice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold yellow]*Vilket typ av konto vill du logga in med?*[/]")
                                .PageSize(10)
                                .AddChoices(new[]
                                {
                                    "PrivatKonto",
                                    "FöretagsKonto"
                                }));

                var konto = bankHanterare.Pinkod(AccChoice);

                if (konto != null)
                {
                    bool BigProgram = true;
                    Console.Clear();

                    // Kontrollera vilken typ av konto och visa rätt meddelande
                    if (AccChoice == "PrivatKonto" && konto is Bankkonto privatKonto)
                    {
                        AnsiConsole.Markup($"[bold green]Välkommen {privatKonto.Namn}![/]\n");
                    }
                    else if (AccChoice == "FöretagsKonto" && konto is FöretagsBankkonton företagsKonto)
                    {
                        AnsiConsole.Markup($"[bold green]Välkommen {företagsKonto.FöretagsNamn}![/]\n");
                    }

                    while (BigProgram)
                    {
                        // Skapa en interaktiv meny med Spectre.Console
                        var choice = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("[bold yellow]---Detta är din Bankmeny---[/]")
                                .PageSize(10)
                                .AddChoices(new[]
                                {
                                    "Kolla ditt saldo",
                                    "Överför pengar",
                                    "Redigera kontoinfo",
                                    "Se historik",
                                    "Logga ut",
                                    "Starta parkering",
                                    "Avsluta parkering",
                                    "Visa pågående parkeringar",
                                    "Stäng av programmet"
                                }));

                        switch (choice)
                        {
                            case "Kolla ditt saldo":
                                if (konto is Bankkonto bankkonto)
                                {
                                    AnsiConsole.Markup($"[bold cyan]Ditt saldo är: {bankkonto.Saldo} kr[/]");
                                }
                                else if(konto is FöretagsBankkonton företagskonto)
                                {
                                    AnsiConsole.Markup($"[bold cyan]Ditt saldo är: {företagskonto.Saldo} kr[/]");
                                }
                                help.Pausa();
                                break;

                            case "Överför pengar":
                                bankHanterare.TransferMoney();
                                help.Pausa();
                                break;

                            case "Redigera kontoinfo":
                                bankHanterare.EditAccountName();
                                help.Pausa();
                                break;

                            case "Se historik":
                                bankHanterare.PrintTransaction(); // Int fullständig
                                help.Pausa();
                                break;

                            case "Logga ut":
                                Console.Clear();
                                AnsiConsole.Markup("[bold yellow]Du har loggat ut.[/]");
                                BigProgram = false;
                                break;

                            case "Starta parkering":
                                Parkering.StartaParkering();
                                break;

                            case "Avsluta parkering": //denna måste fixas
                                Parkering.AvslutaParkering();
                                break;

                            case "Visa pågående parkeringar":
                                Parkering.VisaAktivaParkeringar();
                                break;

                            case "Stäng av programmet":
                                AnsiConsole.Markup("[bold red]Programmet är avslutat.[/]");
                                BigProgram = false;
                                ProgramIsRuning = false;
                                break;

                            default:
                                AnsiConsole.Markup("[bold red]Ogiltigt val. Vänligen försök igen.[/]");
                                break;
                        }
                    }
                }
                else
                {
                    AnsiConsole.Markup("[bold red]Felaktig pinkod. Försök igen.[/]");
                }
            }
        }
    }
}
