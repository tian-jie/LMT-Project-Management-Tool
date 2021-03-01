using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.PublicApi.CreateResourcePlanEndpoints;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.PublicApi.ResporcePlanEndpoints
{

    //[Authorize(Roles = BlazorShared.Authorization.Constants.Roles.ADMINISTRATORS, AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreateOrUpdate : BaseAsyncEndpoint<CreateResourcePlanRequest, CreateResourcePlanResponse>
    {
        private readonly IAsyncRepository<ResourcePlan> _itemRepository;

        public CreateOrUpdate(IAsyncRepository<ResourcePlan> itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpPost("api/resource-plan/create-or-update")]
        [SwaggerOperation(
            Summary = "Creates or Modifies a Resource Plan Item",
            Description = "Creates or Modifies a Resource Plan Item",
            OperationId = "resource-plan.create-or-update",
            Tags = new[] { "ResourcePlanEndpoints" })
        ]
        public override async Task<ActionResult<CreateResourcePlanResponse>> HandleAsync(CreateResourcePlanRequest request, CancellationToken cancellationToken)
        {
            var response = new CreateResourcePlanResponse(request.CorrelationId());

            var newItem = new ResourcePlan()
            {
                Id = request.Id,
                EmployeeGid = request.EmployeeGid,
                ProjectGid = request.ProjectGid,
                TaskName = request.TaskName,
                IsDeleted = false,
                StartDate = DateTime.Parse(request.start_date.Substring(0, 10)),
                EndDate = DateTime.Parse(request.end_date.Substring(0, 10)),
                Workload = request.Workload
            };

            if (request.Id == 0)
            {
                newItem = await _itemRepository.AddAsync(newItem, cancellationToken);
            }
            else
            {
                await _itemRepository.UpdateAsync(newItem, cancellationToken);
            }

            response.Item = newItem;
            return response;
        }

    }
}
