namespace CBRS.Core.Models
{
    public struct BoardConfig
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Schema { get; set; }
        public string Mfo { get; set; }
        public string Sc { get; set; }
        public int Status { get; set; }
        public int NumberLength { get; set; }
        public int CurrencyTypeId { get; set; }
    }
}