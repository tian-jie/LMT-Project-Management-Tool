using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface ISharePointUserService : IBaseService<SharePointUser>
    {
        Task Clear();
    }
}
