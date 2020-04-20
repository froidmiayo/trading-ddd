namespace TradingEngineDDD.Controllers.ModelParam
{
    public class FundTransferParam
    {
        public  int RecipientId { get; set; }
        public  int SenderId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
    }
}