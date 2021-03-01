using Microsoft.eShopWeb.ApplicationCore.Entities;
using System;

namespace Microsoft.eShopWeb.PublicApi.CreateResourcePlanEndpoints
{
    public class CreateResourcePlanResponse : BaseResponse
    {
        public CreateResourcePlanResponse(Guid correlationId) : base(correlationId)
        {
        }

        public CreateResourcePlanResponse()
        {
        }

        public ResourcePlan Item { get; set; }
    }
}
