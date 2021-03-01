using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ClientService : BaseService<Client>, IClientService
    {
        public ClientService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Clear()
        {
            string sql = "truncate table [Client]";
            await SqlExecuteNonQuery(sql);
        }

    }
}
