using System.Collections.Generic;
using System.Threading.Tasks;
using CBRS.Core.Models;

namespace CBRS.Core.Services.Interfaces
{
    public interface IXmlService
    {
        List<Schema> GetSchemas();
    }
}