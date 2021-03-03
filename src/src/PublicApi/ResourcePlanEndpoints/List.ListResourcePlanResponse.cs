using System;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.PublicApi.ResourcePlanEndpoints
{
    public class ListResourcePlanResponse : BaseResponse
    {
        public ListResourcePlanResponse(Guid correlationId) : base(correlationId)
        {
        }

        public ListResourcePlanResponse()
        {
        }

        public List<ResourcePlanItem> Tasks { get; set; } = new List<ResourcePlanItem>();

        public List<ResourceItem> Resources { get; set; } = new List<ResourceItem>();

        public List<CalendarItem> Calendars { get; set; } = new List<CalendarItem>();
    }
}
