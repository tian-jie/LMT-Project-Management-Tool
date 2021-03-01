using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class UserGroupService : BaseService<UserGroup>, IUserGroupService
    {
        public UserGroupService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

        public async Task Clear()
        {
            await SqlExecuteNonQuery("truncate table [UserGroup]");
        }
    }
}
