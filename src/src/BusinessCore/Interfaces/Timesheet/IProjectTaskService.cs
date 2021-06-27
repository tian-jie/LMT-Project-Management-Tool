using Microsoft.eShopWeb.ApplicationCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IProjectTaskService : IBaseService<ProjectTask>
    {
        Task<List<ProjectTask>> GetTasksByProject(string projectGid);

        Task<List<ProjectTask>> GetProjectTaskById(string Gid);

        Task<List<ProjectTask>> GetAllProjectTasks();
    }
}
