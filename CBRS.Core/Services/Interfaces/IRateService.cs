using System.Threading.Tasks;

namespace CBRS.Core.Services.Interfaces
{
    public interface IRateService
    {
        Task RefreshData();
        
    }
}