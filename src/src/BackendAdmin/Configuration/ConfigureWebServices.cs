using MediatR;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Microsoft.eShopWeb.BusinessCore.Services;
using Microsoft.eShopWeb.Web.Interfaces;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.eShopWeb.Web.Configuration
{
    public static class ConfigureWebServices
    {
        public static IServiceCollection AddWebServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(BasketViewModelService).Assembly);
            services.AddScoped<IBasketViewModelService, BasketViewModelService>();
            services.AddScoped<CatalogViewModelService>();
            services.AddScoped<ICatalogItemViewModelService, CatalogItemViewModelService>();
            services.Configure<CatalogSettings>(configuration);
            services.AddScoped<ICatalogViewModelService, CachedCatalogViewModelService>();

            services.AddScoped<ITimesheetService, TimesheetService>();
            services.AddScoped<IClockifyService, ClockifyService>();
            services.AddScoped<IEstimateEffortService, EstimateEffortService>();

            services.AddScoped<IAspNetMenuService, AspNetMenuService>();
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


            return services;
        }
    }
}
