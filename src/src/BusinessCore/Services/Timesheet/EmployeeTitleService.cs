using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class EmployeeTitleService : BaseService<EmployeeTitle>, IEmployeeTitleService
    {
        private List<EmployeeTitle> _employeeTitles = null;

        public EmployeeTitleService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Clear()
        {
            string sql = "delete from EmployeeTitle where Month=" + DateTime.Now.ToString("yyyyMM");
            await SqlExecuteNonQuery(sql);
        }

        public async Task UpdateBySharePointUsers()
        {
            string sql = "update EmployeeTitle from SharePointUser SP, Employee E set FullName = SP.FullName, DisplayName = SP.DisplayName, GivenName=SP.GivenName, OfficeCity = SP.OfficeCity, OfficeCountry = SP.OfficeCountry, FamilyName = SP.FamilyName where Email = SP.Email and EmployeeGId = E.Gid";
            await SqlExecuteNonQuery(sql);
        }

        public async Task<List<EmployeeTitle>> ListByMonth(int year, int month)
        {
            if(_employeeTitles == null)
            {
                _employeeTitles = (await ListAllAsync()).ToList();
            }
            var m = year.ToString() + month.ToString("D2") + "  ";
            return _employeeTitles.Where(a => a.Month == m).ToList();
        }
    }
}
