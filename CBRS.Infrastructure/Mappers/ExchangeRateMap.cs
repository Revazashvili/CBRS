using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CBRS.Core.Models;
using CBRS.Infrastructure.Entities;

namespace CBRS.Infrastructure.Mappers
{
    public static class ExchangeRateMap
    {
        public static Rate AsDomain(this ExchangeRate exchangeRate)
        {
            var rate = new Rate();
            if (exchangeRate is null) return rate;
            rate =  new Rate()
            {
                Iso1 = exchangeRate.CURR1,
                Iso2 = exchangeRate.CURR2,
                Scale = exchangeRate.scale,
                Buy = exchangeRate.RATE_BY,
                Sell = exchangeRate.RATE_sell
            };
            return rate;
        }

        public static IEnumerable<Rate> AsDomain(this IEnumerable<ExchangeRate> exchangeRates)
        {
            var rates = new List<Rate>();
            if (exchangeRates is null || !exchangeRates.Any()) return rates;
            foreach (var exchangeRate in exchangeRates)
            {
                rates.Add(exchangeRate.AsDomain());
            }

            return rates;
        }
    }
}