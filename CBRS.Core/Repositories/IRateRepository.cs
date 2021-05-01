using System.Collections.Generic;
using System.Threading.Tasks;
using CBRS.Core.Models;

namespace CBRS.Core.Repositories
{
    public interface IRateRepository
    {
        Task Insert(IEnumerable<BoardRate> boardRates);
        Task<IEnumerable<Rate>> GetExchangeRates(string mfo,int sc,int type);
        Task<IEnumerable<Rate>> GetMtRates(string mfo,int sc);
        Task<IEnumerable<BoardConfig>> GetBoards();
    }
}