namespace CBRS.Infrastructure.Entities
{
    public class MtRate
    {
        public string PairCode { get; set; }
        public int Scale { get; set; }
        public decimal BuyRate { get; set; }
        public decimal SellRate { get; set; }
    }
}