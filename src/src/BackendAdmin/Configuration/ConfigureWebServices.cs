using MediatR;
using Microsoft.eShopWeb.BackendAdmin.Interfaces;
using Microsoft.eShopWeb.BackendAdmin.ServiceInterfaces;
using Microsoft.eShopWeb.BackendAdmin.Services;
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

            return services;
        }
    }
}
