namespace BANK
{
    public class Parkering
    {
        public string RegNummer { get; }
        public DateTime StartTid { get; }
        public int KostnadPerSekund { get; }


        public Parkering(string regNummer)
        {
            RegNummer = regNummer;
            StartTid = DateTime.Now;
            KostnadPerSekund = 1;
        }

        public decimal GetKostnad()
        {
            var duration = DateTime.Now - StartTid;
            return (decimal)duration.TotalSeconds * KostnadPerSekund;
        }
    }
}