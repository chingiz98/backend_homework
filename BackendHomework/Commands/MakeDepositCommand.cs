namespace BackendHomework.Commands
{
    public class MakeDepositCommand
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}