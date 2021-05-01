using System.Collections.Generic;
using CBRS.Infrastructure.Entities;

namespace CBRS.Infrastructure.Mappers
{
    public static class BoardRateMap
    {
        public static BoardRate AsEntity(this Core.Models.BoardRate boardRate)
        {
            var rate = new BoardRate();
            if (boardRate is null) return rate;
            rate = new BoardRate
            {
                TABLONAME = boardRate.Name,
                CCYRate = boardRate.Rate,
                CCYSchema = boardRate.Schema
            };
            return rate;
        }

        public static IEnumerable<BoardRate> AsEntity(this IEnumerable<Core.Models.BoardRate> boardRates)
        {
            var rates = new List<BoardRate>();
            if (boardRates is null) return rates;
            foreach (var boardRate in boardRates)
            {
                rates.Add(boardRate.AsEntity());
            }

            return rates;
        }
    }
}