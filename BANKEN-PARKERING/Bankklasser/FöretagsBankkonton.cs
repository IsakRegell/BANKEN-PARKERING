namespace BANK
{
    public class F�retagsBankkonton
    {
        public string? F�retagsNamn { get; set; }
        public int OrgenisationsNummer { get; set; }
        public int Saldo { get; set; }
        public int Pinkod { get; set; }

        public F�retagsBankkonton(string f�retagsNamn, int orgenisationsNummer, int saldo, int pinkod)
        {
            F�retagsNamn = f�retagsNamn;
            OrgenisationsNummer = orgenisationsNummer;
            Saldo = saldo;
            Pinkod = pinkod;
        }
    }
}
