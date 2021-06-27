using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Interfaces
{
    public interface IProjectService : IBaseService<Project>
    {
        Task Clear();


        Task<List<Project>> GetAllProjects();

        //Task<List<Project>> GetAllActiveProjects();

        Task<Project> GetProjectById(string Gid);

        ProjectAccountingView AccountProject(string Gid);

        Task<ProjectAccountingView> GetProjectEstimatedEffortById(string Gid);


    }
}
