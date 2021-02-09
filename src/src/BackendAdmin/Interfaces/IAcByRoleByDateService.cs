using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface IEffortUsedByRoleByDateService : IBaseService<EffortUsedByRoleByDate>
    {
        Task<List<EffortUsedViewModel>> GetEffortUsedByDay(string projectGid, string taskGid);

        Task<List<EffortUsedViewModel>> GetEffortUsedByRole(string projectGid, string taskGid);

        Task<List<EffortUsedViewModel>> GetEffortUsedByRoleCategory(string projectGid, string taskGid);
    }
}
