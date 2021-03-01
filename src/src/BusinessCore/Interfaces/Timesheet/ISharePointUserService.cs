using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface ISharePointUserService : IBaseService<SharePointUser>
    {
        Task Clear();
    }
}
