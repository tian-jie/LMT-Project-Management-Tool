using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ProjectTaskService : BaseService<ProjectTask>, IProjectTaskService
    {
        public ProjectTaskService(CatalogContext dbContext)
            : base(dbContext)
        {

        }

        public async Task<List<ProjectTask>> GetTasksByProject(string projectGid)
        {
            var tasks =  await WhereAsync(a => a.ProjectGid == projectGid);

            return tasks.ToList();
        }

        public async Task<List<ProjectTask>> GetProjectTaskById(string Gid)
        {
            var tasks = await WhereAsync(a => a.IsDeleted != true && a.Gid == Gid);

            return tasks.ToList();
        }

        public async Task<List<ProjectTask>> GetAllProjectTasks()
        {
            var tasks = await WhereAsync(a => a.IsDeleted != true);

            return tasks.ToList();
        }

    }
}
