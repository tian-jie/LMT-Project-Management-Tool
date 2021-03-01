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

        public List<ResourcePlanItem> tasks { get; set; } = new List<ResourcePlanItem>();
        public List<ResourceItem> resources { get; set; } = new List<ResourceItem>();
    }
}
