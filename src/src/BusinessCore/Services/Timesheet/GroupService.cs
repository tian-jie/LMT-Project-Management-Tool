using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class GroupService : BaseService<Group>, IGroupService
    {
        public GroupService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

        public async Task Clear()
        {
            await SqlExecuteNonQuery("truncate table [Group]");
        }

    }
}
