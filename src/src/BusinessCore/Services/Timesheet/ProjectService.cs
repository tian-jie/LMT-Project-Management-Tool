using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ProjectService : BaseService<Project>, IProjectService
    {
        IEstimateEffortService _estimateEffortService;
        //ITaskToProjectMappingService _taskToProjectMappingService;


        public ProjectService(CatalogContext dbContext,
            IEstimateEffortService estimateEffortService)
            : base(dbContext)
        {
            _estimateEffortService = estimateEffortService;
        }

        public async Task Clear()
        {
            await SqlExecuteNonQuery("truncate table [Project]");
        }


        public async Task<List<Project>> GetAllProjects()
        {
            var projects = (await WhereAsync(a => a.IsDeleted != true)).ToList();

            // 再加上其他以task为项目的项目
            //var taskAsProjects = _taskToProjectMappingService.AllProjects();
            //projects.AddRange(taskAsProjects);

            return projects;
        }

        public async Task<List<Project>> GetAllActiveProjects()
        {
            var projects = (await WhereAsync(a => a.IsDeleted != true & a.Archived == false)).ToList();

            //// 再加上其他以task为项目的项目
            //var taskAsProjects = _taskToProjectMappingService.AllProjects();
            //projects.AddRange(taskAsProjects);

            return projects;
        }

        public async Task<Project> GetProjectById(string Gid)
        {
            var project = (await WhereAsync(a => a.IsDeleted != true && a.Gid == Gid)).FirstOrDefault();

            //if (project == null)
            //{
            //    var taskAsProject = _taskToProjectMappingService.Repository.Entities.FirstOrDefault(a => a.IsDeleted != true && a.ProjectGid == Gid);
            //    project = new Project()
            //    {
            //        Id = 0,
            //        Name = taskAsProject.ProjectName,
            //        Gid = Gid
            //    };
            //}

            return project;
        }

        public async Task<ProjectAccountingView> GetProjectEstimatedEffortById(string Gid)
        {
            var efforts = (await _estimateEffortService.WhereAsync(a => a.ProjectGid == Gid)).ToList();
            var estimatedSpentManHour = efforts.Sum(a => a.Effort);
            var estimatedSpentManHourRate = efforts.Sum(a => (a.Effort * a.RoleRate));

            return new ProjectAccountingView()
            {
                ProjectGid = Gid,
                EstimatedSpentManHour = estimatedSpentManHour,
                EstimatedSpentManHourRate = estimatedSpentManHourRate
            };
        }

        public ProjectAccountingView AccountProject(string Gid)
        {
            // 第一步，获取这个项目的所有clockify信息
            //_timeEntryService

            return null;
        }

    }
}
