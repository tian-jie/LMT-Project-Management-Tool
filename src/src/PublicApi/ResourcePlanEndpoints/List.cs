using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.PublicApi.ResourcePlanEndpoints
{
    public class List : BaseAsyncEndpoint<ListResourcePlanResponse>
    {
        private readonly IResourcePlanService _resourcePlanService;
        private readonly IEmployeeService _employeeService;


        public List(IResourcePlanService resourcePlanService, IEmployeeService employeeService)
        {
            _resourcePlanService = resourcePlanService;
            _employeeService = employeeService;
        }

        [HttpGet("api/resource-plan/list/{Id}")]
        [SwaggerOperation(
            Summary = "List Resource Plan Items",
            Description = "List Resource Plan Items",
            OperationId = "resource-plan.List",
            Tags = new[] { "ResourcePlanEndpoints" })
        ]
        public override async Task<ActionResult<ListResourcePlanResponse>> HandleAsync(CancellationToken cancellationToken)
        {
            var response = new ListResourcePlanResponse();
            var queryId = Request.RouteValues["Id"].ToString();
            var items = (await _resourcePlanService.WhereAsync(a => a.ProjectGid == queryId)).ToList();
            var employees = await _employeeService.ListAllAsync(cancellationToken);

            response.tasks.AddRange(items.Select(a => new ResourcePlanItem()
            {
                id = a.Id,
                TaskName = a.TaskName,
                start_date = a.StartDate.ToString("yyyy-MM-dd"),
                end_date = a.EndDate.ToString("yyyy-MM-dd"),
                duration = (int)(a.EndDate - a.StartDate).TotalDays,
                owner_id = a.EmployeeGid,
                ProjectGid = a.ProjectGid,
                EmployeeGid = a.EmployeeGid,
                open = true,
                progress = 0,
                Workload = a.Workload

            }));

            var taskResources = items.Select(a => a.EmployeeGid).Distinct();

            //employees = employees.Where(a => taskResources.Contains(a.Gid)).ToList();

            response.resources.AddRange(employees.Select(a => new ResourceItem()
            {
                key = a.Gid,
                label = a.DisplayName,
                backgroundColor = GetColorStringByString(a.FullName),
                textColor = GetInvertColorStringByString(a.FullName)
            }));

            return Ok(response);
        }

        private string GetColorStringByString(string text)
        {
            var colorNums = GetColorByString(text);

            return $"#{colorNums[0].ToString("X").PadLeft(2, '0')}{colorNums[1].ToString("X").PadLeft(2, '0')}{colorNums[2].ToString("X").PadLeft(2, '0')}";

        }

        private static int[] GetColorByString(string text)
        {
            // 先md5，然后给md5拆成3份，每份的字母数字加起来对255取余
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytValue, bytHash;
            bytValue = System.Text.Encoding.UTF8.GetBytes(text);
            bytHash = md5.ComputeHash(bytValue);
            md5.Clear();

            var num1 = 0;
            var num2 = 0;
            var num3 = 0;
            for (int i = 0; i < bytHash.Length - 3; i += 3)
            {
                num1 += bytHash[i];
                if (num1 >= 256)
                {
                    num1 -= 256;
                }
                num2 += bytHash[i + 1];
                if (num2 >= 256)
                {
                    num2 -= 256;
                }
                num3 += bytHash[i + 2];
                if (num3 >= 256)
                {
                    num3 -= 256;
                }
            }

            return new int[] { num1, num2, num3 };
        }

        private string GetInvertColorStringByString(string text)
        {
            var colorNums = GetColorByString(text);

            return $"#{(255 - colorNums[0]).ToString("X").PadLeft(2, '0')}{(255 - colorNums[1]).ToString("X").PadLeft(2, '0')}{(255 - colorNums[2]).ToString("X").PadLeft(2, '0')}";

        }
    }
}
