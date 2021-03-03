using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.ResourcePlanEndpoints
{
    public class CalendarItem
    {
        public string Country { get; set; }

        public List<HolidayItem> Holidays { get; set; } = new List<HolidayItem>();
    }

    public class HolidayItem
    {
        public DateTime Date { get; set; }

        public bool IsWorkingDay { get; set; }

        public string Description { get; set; }
    }
}
