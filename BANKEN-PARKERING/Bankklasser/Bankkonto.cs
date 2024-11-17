namespace BANK
{
    public class Bankkonto
    {
        public string? Namn { get; set; }
        public int Kontonummer { get; set; }
        public int Saldo { get; set; }
        public int Pinkod { get; set; }

        public Bankkonto(string namn, int kontonummer, int saldo, int pinkod)
        {
            Namn = namn;
            Kontonummer = kontonummer;
            Saldo = saldo;
            Pinkod = pinkod;
        }

    }
}