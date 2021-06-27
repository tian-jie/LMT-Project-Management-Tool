using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class YearWeekCalendar : BaseEntity, IAggregateRoot
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 年份
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// 当年的第几周
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// 本周的开始日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 本周的结束日期
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
