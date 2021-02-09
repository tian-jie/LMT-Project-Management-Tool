using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using Microsoft.eShopWeb.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Services
{
    public class EmployeeService : BaseService<Employee>, IEmployeeService
    {
        public EmployeeService(CatalogContext timesheetContext)
            : base(timesheetContext)
        {

        }

        public async Task Clear()
        {
            string sql = "truncate table [Employee]";
            await SqlExecuteNonQuery(sql);
        }

        public async Task<List<EmployeeViewModel>> AllEmployeesWithRole()
        {
            //            var sql = @"select E.*, R.Id RoleId, R.Title RoleName, R.Rate RoleRate
            //from Employee E, RoleTitle R, EmployeeRate ER
            //where E.GID = ER.EmployeeGid and R.ID = ER.RoleId and E.isdeleted<>1 and ER.isdeleted<>1 and R.isdeleted<>1
            //";

            return await Task.Run(() =>
            {

                var employees = from e in _dbContext.Employee
                                join er in _dbContext.EmployeeTitle on e.Gid equals er.EmployeeGid
                                join rt in _dbContext.RoleTitle on er.RoleId equals rt.Id
                                where e.IsDeleted.Value != true && er.IsDeleted.Value != true && rt.IsDeleted.Value != true
                                select new EmployeeViewModel
                                {
                                    Id = e.Id,
                                    Gid = e.Gid,
                                    Name = e.FullName,
                                    Email = e.Email,
                                    Status = e.Status,
                                    ProfilePicture = e.ProfilePicture,
                                    RoleId = er.RoleId,
                                    RoleName = rt.Title,
                                    RoleRate = rt.Rate
                                };

                return employees.ToList();
            });
        }

        public async Task UpdateBySharePointUsers()
        {
            string sql = "update Employee from SharePointUser SP set FullName = SP.FullName, DisplayName = SP.DisplayName, GivenName=SP.GivenName, OfficeCity = SP.OfficeCity, OfficeCountry = SP.OfficeCountry, FamilyName = SP.FamilyName where SP.Email = E.Email";
            await SqlExecuteNonQuery(sql);
        }
    }
}
