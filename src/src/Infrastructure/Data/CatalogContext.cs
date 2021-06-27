using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Entities.BasketAggregate;
using Microsoft.eShopWeb.ApplicationCore.Entities.OrderAggregate;
using System.Reflection;

namespace Microsoft.eShopWeb.Infrastructure.Data
{

    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Basket> Baskets { get; set; }
        public DbSet<CatalogItem> CatalogItems { get; set; }
        public DbSet<CatalogBrand> CatalogBrands { get; set; }
        public DbSet<CatalogType> CatalogTypes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }


        // Timesheet

        public DbSet<Client> Client { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeTitle> EmployeeTitle { get; set; }
        public DbSet<EstimateEffort> EstimateEffort { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<ProjectTask> ProjectTask { get; set; }
        public DbSet<ProjectUser> ProjectUser { get; set; }
        public DbSet<ResourcePlan> ResourcePlan { get; set; }
        public DbSet<RoleTitle> RoleTitle { get; set; }
        public DbSet<TaskToProjectMapping> TaskToProjectMapping { get; set; }
        public DbSet<TimeEntry> TimeEntry { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<SharePointUser> SharePointUser { get; set; }
        public DbSet<EffortUsedByRoleByDate> AcByRoleByDate { get; set; }
        public DbSet<AspNetMenu> AspNetMenu { get; set; }

        public DbSet<ProjectOwner> ProjectOwner { get; set; }
        public DbSet<WorkCalendar> WorkCalendar { get; set; }
        public DbSet<OrgChart> OrgChart { get; set; }
        public DbSet<YearWeekCalendar> YearWeekCalendar { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
