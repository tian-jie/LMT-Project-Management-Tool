using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class WorkCalendarService : BaseService<WorkCalendar>, IWorkCalendarService
    {
        public WorkCalendarService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

    }
}
