using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        public ClientService(CatalogContext timesheetContext)
            : base(timesheetContext)
        {
        }

        public async Task Clear()
        {
            string sql = "truncate table [Client]";
            await SqlExecuteNonQuery(sql);
        }

    }
}
