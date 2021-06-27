using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.BusinessCore.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IEmployeeService : IBaseService<Employee>
    {
        Task Clear();
        Task UpdateBySharePointUsers();


         //<summary>
         //获取员工，带对应的角色
         //</summary>
         //<returns></returns>
        Task<IList<EmployeeViewModel>> AllEmployeesWithRole();

    }
}
