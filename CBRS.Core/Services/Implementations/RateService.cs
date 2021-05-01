using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CBRS.Core.Models;
using CBRS.Core.Models.Enums;
using CBRS.Core.Repositories;
using CBRS.Core.Services.Interfaces;
using CBRS.Helper;

namespace CBRS.Core.Services.Implementations
{
    public class RateService : IRateService
    {
        private readonly IRateRepository _rateRepository;
        private readonly IXmlService _xmlService;

        public RateService(IRateRepository rateRepository,IXmlService xmlService)
        {
            _rateRepository = rateRepository;
            _xmlService = xmlService;
        }
        public async Task RefreshData()
        {
            var schemas = _xmlService.GetSchemas();
            var boardConfigs = await _rateRepository.GetBoards();
            var boardRates = await GenerateRates(boardConfigs, schemas);
            await _rateRepository.Insert(boardRates);
        }
        private async Task<List<BoardRate>> GenerateRates(IEnumerable<BoardConfig> boardConfigs, List<Schema> schemas)
        {
            var boardRates = new List<BoardRate>();;
            foreach (var boardConfig in boardConfigs)
            {
                var validSchema = schemas.FirstOrDefault(x => string.Equals(x.Name, boardConfig.Schema, StringComparison.CurrentCultureIgnoreCase));
                if (validSchema is null) continue;
                if (boardConfig.CurrencyTypeId == (int) CurrencyType.Exchange)
                    boardRates.Add(await GetBoardRateFromExchangeRate(boardConfig, validSchema));
                else if (boardConfig.CurrencyTypeId == (int) CurrencyType.Mt)
                    boardRates.Add(await GetBoardRateFromMtRate(boardConfig, validSchema));
            }

            return boardRates;
        }
        private async ValueTask<BoardRate> GetBoardRateFromExchangeRate(BoardConfig boardConfig,Schema schema)
        {
            var exchangeRates = (await _rateRepository.GetExchangeRates(boardConfig.Mfo,Convert.ToInt32(boardConfig.Sc),1)).ToArray();
            
            string value = string.Empty;
            foreach (var pair in schema.Pairs)
            {
                var rate = exchangeRates.FirstOrDefault(x => x.Iso1.ToLower() == pair.Iso1.ToLower() && x.Iso2.ToLower() == pair.Iso2.ToLower());
                if (rate is not null)
                    value += $"{rate.Iso1}/{rate.Iso2} " +
                             $"{CurrencyHelper.get_cur5(rate.Buy.ToString(CultureInfo.InvariantCulture),rate.Scale.ToString(),boardConfig.NumberLength.ToString())}/" +
                             $"{CurrencyHelper.get_cur5(rate.Sell.ToString(CultureInfo.InvariantCulture),rate.Scale.ToString(),boardConfig.NumberLength.ToString())};";
            }
            value = value.Substring(0, value.Length - 1); // remove last ';'
            //Example: USD/GEL 3.0350/3.0800;EUR/GEL 3.4050/3.4990;RUB/GEL 0.0417/0.0442;TRL/GEL 0.3620/0.4560
            return new BoardRate
            {
                Name = boardConfig.Name,
                Schema = boardConfig.Schema,
                Rate = value
            };
        }
        private async ValueTask<BoardRate> GetBoardRateFromMtRate(BoardConfig boardConfig,Schema schema)
        {
            var exchangeRates = (await _rateRepository.GetMtRates(boardConfig.Mfo,Convert.ToInt32(boardConfig.Sc))).ToArray();
            string value = string.Empty;
            foreach (var pair in schema.Pairs)
            {
                var rate = exchangeRates.FirstOrDefault(x => x.Iso1.ToLower() == pair.Iso1.ToLower() && x.Iso2.ToLower() == pair.Iso2.ToLower());
                if (rate is not null)
                    value += $"{rate.Iso1}/{rate.Iso2} " +
                             $"{CurrencyHelper.get_cur5(rate.Buy.ToString(CultureInfo.InvariantCulture), rate.Scale.ToString(), boardConfig.NumberLength.ToString())}/" +
                             $"{CurrencyHelper.get_cur5(rate.Sell.ToString(CultureInfo.InvariantCulture), rate.Scale.ToString(), boardConfig.NumberLength.ToString())};";
            }

            value = value.Substring(0, value.Length - 1);
            return new BoardRate
            {
                Name = boardConfig.Name,
                Schema = boardConfig.Schema,
                Rate = value
            };
        }
    }
}