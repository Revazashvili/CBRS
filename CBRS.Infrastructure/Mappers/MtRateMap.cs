using System.Collections.Generic;
using System.Linq;
using CBRS.Core.Models;
using CBRS.Infrastructure.Entities;

namespace CBRS.Infrastructure.Mappers
{
    public static class MtRateMap
    {
        public static Rate AsDomain(this MtRate mtRate)
        {
            var rate = new Rate();
            if (mtRate is null) return rate;
            string[] isos = mtRate.PairCode.Split('/');
            rate = new Rate
            {
                Iso1 = isos[0],
                Iso2 = isos[1],
                Scale = mtRate.Scale,
                Buy = mtRate.BuyRate,
                Sell = mtRate.SellRate
            };
            return rate;
        }

        public static IEnumerable<Rate> AsDomain(this IEnumerable<MtRate> mtRates)
        {
            var rates = new List<Rate>();
            if (mtRates is null || !mtRates.Any()) return rates;
            foreach (var mtRate in mtRates)
            {
                rates.Add(mtRate.AsDomain());
            }

            return rates;
        }
    }
}