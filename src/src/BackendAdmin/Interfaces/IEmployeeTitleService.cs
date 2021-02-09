using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Interfaces
{
    public interface IEmployeeTitleService : IBaseService<EmployeeTitle>
    {
        Task Clear();
        Task UpdateBySharePointUsers();

        Task<List<EmployeeTitle>> ListByMonth(int year, int month);
    }
}
