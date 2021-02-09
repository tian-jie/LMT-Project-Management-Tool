using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class GroupService : BaseService<Group>, IGroupService
    {
        public GroupService(CatalogContext timesheetContext)
            : base(timesheetContext)
        {

        }

        public async Task Clear()
        {
            await SqlExecuteNonQuery("truncate table [Group]");
        }

    }
}
