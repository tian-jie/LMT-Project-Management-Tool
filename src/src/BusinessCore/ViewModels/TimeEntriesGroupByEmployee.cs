using System.Collections.Generic;

namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    public class TimeEntriesGroupByEmployeeView
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public string EmployeeName { get; set; }

        public string EmployeeRole { get; set; }

        public decimal EmployeeRate { get; set; }

        public decimal TotalHours { get; set; }

        public decimal TotalHoursRate { get; set; }

        public List<TotalEffortByWeek> TotalEffortByWeek { get; set; }

    }

    public class TotalEffortByWeek
    {
        public int Year { get; set; }
        public int WeekNumber { get; set; }
        public string WeekDescription { get; set; }
        public decimal TotalHours { get; set; }
        public decimal TotalHoursRate { get; set; }

    }
}
