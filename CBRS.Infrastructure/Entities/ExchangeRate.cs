using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CBRS.Infrastructure.Entities
{
    public class ExchangeRate
    {
        public string CURR1 { get; set; }
        public string CURR2 { get; set; }
        public int scale { get; set; }
        public decimal RATE_BY { get; set; }
        public decimal RATE_sell { get; set; }
    }
}