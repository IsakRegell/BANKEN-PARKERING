namespace BANK
{
    public class Transaction
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()// Denna Customizear Outputen
        {
            return $"From: {FromAccount}, To: {ToAccount}, Amount: {Amount}kr, Date: {Date}";
        }
    }
}