namespace BANK
{
    public class FöretagsBankkonton
    {
        public string? FöretagsNamn { get; set; }
        public int OrgenisationsNummer { get; set; }
        public int Saldo { get; set; }
        public int Pinkod { get; set; }

        public FöretagsBankkonton(string företagsNamn, int orgenisationsNummer, int saldo, int pinkod)
        {
            FöretagsNamn = företagsNamn;
            OrgenisationsNummer = orgenisationsNummer;
            Saldo = saldo;
            Pinkod = pinkod;
        }
    }
}
