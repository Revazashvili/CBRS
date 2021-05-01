using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using CBRS.Core.Repositories;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CBRS.Core.Models;
using CBRS.Infrastructure.Entities;
using CBRS.Infrastructure.Mappers;
using Dapper;
using Microsoft.Extensions.Logging;
using BoardRate = CBRS.Core.Models.BoardRate;

namespace CBRS.Infrastructure.Repositories
{
    public class RateRepository : IRateRepository 
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<RateRepository> _logger;

        public RateRepository(IConfiguration configuration,ILogger<RateRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Insert(IEnumerable<BoardRate> boardRates)
        {
            var dbConnection = await RaiseConnection();
            await using var transaction =await dbConnection.BeginTransactionAsync();
            try
            {
                var query = "delete from dbo.RATES_FOR_TABLO;";
                await dbConnection.ExecuteAsync(query,transaction,commandTimeout:30000,commandType:CommandType.Text,transaction:transaction);
                await dbConnection.ExecuteAsync(
                    "insert into dbo.RATES_FOR_TABLO(TABLONAME,CCYSCHEMA,CCYRATE) values (@TABLONAME,@CCYSCHEMA,@CCYRATE)",
                    boardRates.AsEntity(),
                    transaction);
                await transaction.CommitAsync();
                await dbConnection.CloseAsync();
            }
            catch(Exception exception)
            {
                _logger.LogError("Error while inserting rates in database");
                await transaction.RollbackAsync();
                throw new Exception("Error while inserting rates in database");
            }
        }
        public async Task<IEnumerable<Rate>> GetExchangeRates(string mfo, int sc,int type)
        {
            var dbConnection = await RaiseConnection();
            var response = await dbConnection.QueryAsync<ExchangeRate>("sp_get_current_rates_sc_list_all",
                new {mfo = mfo,sc = sc,type = type},
                commandType:CommandType.StoredProcedure);
            await dbConnection.CloseAsync();
            return response.AsDomain();
        }
        public async Task<IEnumerable<Rate>> GetMtRates(string mfo, int sc)
        {
            var dbConnection = await RaiseConnection();
            var response = await dbConnection.QueryAsync<MtRate>("prc_get_mt_current_rates",
                new {mfo = mfo,sc = sc},
                commandType:CommandType.StoredProcedure);
            await dbConnection.CloseAsync();
            return response.AsDomain();
        }
        public async Task<IEnumerable<BoardConfig>> GetBoards()
        {
            var dbConnection = await RaiseConnection();
            const string query = "select TABLONAME,TABLO_IP,CCYSCHEMA,mfo,sc,status,number_len,CurrencyTypeId from RATES_TABLO_Schema where status=1";
            var response = (await dbConnection.QueryAsync<dynamic>(query)).Select(x => new BoardConfig()
            {
                Name = x.TABLONAME,
                Ip = x.TABLO_IP,
                Schema = x.CCYSCHEMA,
                Mfo = x.mfo,
                Sc = x.sc,
                NumberLength = x.number_len,
                Status = x.status,
                CurrencyTypeId = x.CurrencyTypeId
            }).ToList();
            await dbConnection.CloseAsync();
            return response;
        }
        private async Task<SqlConnection> RaiseConnection()
        {
            var dbConnection = GetDbConnection();
            if (dbConnection.State == ConnectionState.Closed)
                await dbConnection.OpenAsync();
            return dbConnection;
        }
        private SqlConnection GetDbConnection()
        {
            return new (_configuration.GetConnectionString("Rate"));
        }
    }
}