using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        Task Clear();
        Task UpdateBySharePointUsers();
    }
}
