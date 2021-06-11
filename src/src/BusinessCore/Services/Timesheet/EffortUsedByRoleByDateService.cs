using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class EffortUsedByRoleByDateService : BaseService<EffortUsedByRoleByDate>, IEffortUsedByRoleByDateService
    {
        public EffortUsedByRoleByDateService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        //public new Task<int> SqlExecuteNonQuery(string sql, CancellationToken cancellationToken = default)
        //{
        //    throw new Exception("Not allowed.");
        //}


        public async Task<List<EffortUsedViewModel>> GetEffortUsedByDay(string projectGid, string taskGid)
        {
            // TODO: 好像这里有性能问题？？？
            var effortsQuery = await WhereAsync(a => a.ProjectGid == projectGid && (taskGid == null || a.TaskGid == taskGid));
            //if (!string.IsNullOrEmpty(taskGid))
            //{
            //    effortsQuery = effortsQuery.Where(a => a.TaskGid == taskGid);
            //}
            var efforts = effortsQuery
                .GroupBy(b => b.Date)
                .Select(a => new EffortUsedViewModel
                {
                    Date = a.Key,
                    TotalHours = a.Sum(a => a.TotalHours),
                    TotalHoursRate = a.Sum(a => a.TotalHoursRate)
                }).ToList();

            return efforts;
        }

        public async Task<List<EffortUsedViewModel>> GetEffortUsedByRole(string projectGid, string taskGid)
        {
            var effortsQuery = await WhereAsync(a => a.ProjectGid == projectGid);
            if (!string.IsNullOrEmpty(taskGid))
            {
                effortsQuery = effortsQuery.Where(a => a.TaskGid == taskGid);
            }
            var efforts = effortsQuery
                .GroupBy(b => b.RoleName)
                .Select(a => new EffortUsedViewModel
                {
                    RoleName = a.Key,
                    TotalHours = a.Sum(a => a.TotalHours),
                    TotalHoursRate = a.Sum(a => a.TotalHoursRate)
                }).ToList();

            return efforts;
        }

        public async Task<List<EffortUsedViewModel>> GetEffortUsedByRoleCategory(string projectGid, string taskGid)
        {
            var effortsQuery = await WhereAsync(a => a.ProjectGid == projectGid);
            if (!string.IsNullOrEmpty(taskGid))
            {
                effortsQuery = effortsQuery.Where(a => a.TaskGid == taskGid);
            }
            var efforts = effortsQuery
                .GroupBy(b => b.RoleCategory)
                .Select(a => new EffortUsedViewModel
                {
                    RoleName = a.Key,
                    TotalHours = a.Sum(a => a.TotalHours),
                    TotalHoursRate = a.Sum(a => a.TotalHoursRate)
                }).ToList();

            return efforts;
        }



    }
}
