using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Data;
using System;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class YearWeekCalendarService : BaseService<YearWeekCalendar>, IYearWeekCalendarService
    {
        public YearWeekCalendarService(CatalogContext dbContext)
            : base(dbContext)
        {
        }

        public async Task Clear()
        {
            await Task.Delay(1);
            throw new InvalidOperationException();
        }
    }
}
