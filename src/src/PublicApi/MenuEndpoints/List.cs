using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.PublicApi.MenuEndpoints
{
    public class List : BaseAsyncEndpoint<ListMenuResponse>
    {
        private readonly IAspNetMenuService _aspNetMenuService;

        public List(IAspNetMenuService aspNetMenuService)
        {
            _aspNetMenuService = aspNetMenuService;
        }

        [HttpGet("api/v1/menu/list")]
        [SwaggerOperation(
            Summary = "List Menu Items",
            Description = "List Mneu Items",
            OperationId = "menu.List",
            Tags = new[] { "MenuEndpoints" })
        ]
        public override async Task<ActionResult<ListMenuResponse>> HandleAsync(CancellationToken cancellationToken)
        {
            var response = new ListMenuResponse();
            var menus = (await _aspNetMenuService.WhereAsync(a => !a.IsDeleted)).ToList();

            response.Menus.AddRange(menus);

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
                num2 += bytHash[i + 1];
                num3 += bytHash[i + 2];

                num1 = num1 & 255;
                num2 = num2 & 255;
                num3 = num3 & 255;
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
