using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Microsoft.eShopWeb.BusinessCore.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopWeb.PublicApi.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<IClockifyService, ClockifyService>();

            services.AddScoped<IAspNetMenuService, AspNetMenuService>();
            services.AddScoped<IAspNetUserService, AspNetUserService>();


            services.AddScoped<IProjectOwnerService, ProjectOwnerService>();
            services.AddScoped<IResourcePlanService, ResourcePlanService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<IProjectTaskService, ProjectTaskService>();
            services.AddScoped<ITimeEntryService, TimeEntryService>();
            services.AddScoped<IUserGroupService, UserGroupService>();
            services.AddScoped<IRoleTitleService, RoleTitleService>();
            services.AddScoped<ISharePointUserService, SharePointUserService>();
            services.AddScoped<IEmployeeTitleService, EmployeeTitleService>();
            services.AddScoped<IEffortUsedByRoleByDateService, EffortUsedByRoleByDateService>();
            services.AddScoped<IWorkCalendarService, WorkCalendarService>();
            services.AddScoped<IEstimateEffortService, EstimateEffortService>();

            return services;
        }
    }
}
