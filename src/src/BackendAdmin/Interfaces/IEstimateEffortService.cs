using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface IEstimateEffortService : IBaseService<EstimateEffort>
    {
        Task DeleteByProject(string projectGid);

        Task<int> Update(string projectGid, List<EstimateEffort> efforts);
    }
}
