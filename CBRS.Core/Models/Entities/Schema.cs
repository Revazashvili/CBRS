using System.Collections.Generic;

namespace CBRS.Core.Models
{
    public class Schema
    {
        public string Name { get; set; }
        public int PairCount { get; set; }
        public List<CcyPair> Pairs { get; set; }
    }
}