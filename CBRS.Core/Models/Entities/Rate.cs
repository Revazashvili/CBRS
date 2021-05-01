namespace CBRS.Core.Models
{
    public class Rate
    {
        public string Iso1 { get; set; }
        public string Iso2 { get; set; }
        public int Scale { get; set; }
        public decimal Buy { get; set; }
        public decimal Sell { get; set; }
    }
}