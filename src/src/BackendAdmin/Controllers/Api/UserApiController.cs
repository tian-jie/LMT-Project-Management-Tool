using Microsoft.AspNetCore.Mvc;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.Interfaces;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Microsoft.eShopWeb.BusinessCore.ViewModel;
using Microsoft.eShopWeb.Web.Controllers.Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.BackendAdmin.Controllers
{
    public class UserApiController : BaseApiController
    {
        private readonly IClockifyService _clockifyService;
        private readonly ITimesheetService _timesheetService;
        private readonly ISharePointUserService _sharePointUserService;
        private readonly IOrgChartService _orgChartService;

        public UserApiController(IClockifyService clockifyService,
            ITimesheetService timesheetService,
            ISharePointUserService sharePointUserService,
            IOrgChartService orgChartService)
        {
            _clockifyService = clockifyService;
            _timesheetService = timesheetService;
            _sharePointUserService = sharePointUserService;
            _orgChartService = orgChartService;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrgChart(string headUserId, string cookies)
        {
            /*
accept:application/json;odata=verbose
accept-encoding:gzip, deflate, br
accept-language:zh-CN,zh;q=0.9,en;q=0.8
cookie:MUID=2B4C992E247364390CF4893E25BB6502; X-Delve-AuthKor=SlkPj56Oumu9TtqdC4GjBu0zdFOgAGfqud_gz2opVa8m5fyPl3LWbrV8dY1VD1yjq22KLyWA24pyX_EEOV72RwLyP8SoyqdYATGwFOIj6v8l_-zFVDKH_Oc77o98QsFkhAFe28eX795WPEEiVSl6HXgD7MZj5bliVzN34QC_zjPlG6vLm3Z2Fo7yen7u7XgozrBahc3j0bwnWGS4_1IiMPD1nqQSPE2oddOfitj8uvi9wnSOpBkLDw2PVMMkLjfBYUn3RIgY7N-83ee_OoBfdHO7Kd_DmfnhF444Pj8Z2UJKl8Fu9ei6wwshfYbrLmuPE5MsglTPtWhzqA1bA1R6yrHvX7iox2rlNQkwGUiXHly-Iez3Rvm3sKfTfUrZ7j1IpvURxytAo2AYkT2ioyWTkEWnR7mu5KC0s_lHgumdDw8sq2QR3qbDSjoIgD_Hqo45iebPRtz2GwEdodbV3hqZ5oS_yxQFKqWj666VgtmMaUlYKe2XvA2TXngAybPXKQLeFXzWdAsGUVpak0xlyroyjra_zaeo1V7yNgE9kk45SgMNXSi5_uBrZETlnnOwvk7qw9OEhTAM03F4mlqcVRQ8Zy4tfhuRkbpepUSZYHQHzN9Zf4gHiJlSKOr9fG7U5c4dsPfa5qW1875geauJJL0ZUtwYizTFTwTSF7uvTfalvAbqmsCeR-V1cRX_NW706Iu1Wh29JMDlAyTrLrOdKtbC47z5ipunnAZBF4gaEAObjc4e7ktxWf6INq5dYAA7LsbOWTymdSMyEavYHrsy7QRCIE5_nV2JZJN7e8M7zCceTTOxddur1uP1v7kI5NkppgeLA2zxTMfssvx0ooKRvBf8cxFIgDiA5GaDpWhP_TL7Yn3Inc4DpsmGIyLaVVs8UOTCv8tqBmM9ZsrajhPIXH8Ypdp5YMcJnn6jG5VvCT3UuDw-vJSn8ZOreaadsyiTb2_qE0lJnIPLPdeurdZ61UxIdZYQHgTQU0jysTptwzL5xKLFqiSdwY9_EESGwiW5CMbaHAfslNJgvCPb9WbXDOJSq2y7PgEJztN4TXq-c_qz2zZAZXv-Wue_C8BOa-XUE3yZeR99khdi-LagrP_IubPnHbXGMq6tRr9oXOkqdq40bwfGQQjaTXxzuSTiG-Sf3CMLtv76tHFmgmWTL4xgD_Yy1HUABw_gyBxNuqdq9wju8HFdhB9Xdhdw95dApQr5GjQY7zNegSVJz8A5C1cBJajFQ60K59rJeOeCs15eP1SGxNRnMhfS_rqygJcMWtaiizc63-tIXkV6Mecz4zBHSZNYyOOFEucGouqtUQhWkhekFsRx1SprkgHNROLfWscvIuXsdLlLY6n5E2NjKJESE5xugwTr5uRKHm2JJXYa-QjlgWkXKNYKa5KShF3dIwjfeaQv-wywMzZqZmFKqaC_nIZnLXWopiWE1IeCxpFRV7_pK3HHj-c2ilH_puYh1ya0wUwWliSFsKOF3vuIoHaqmecn8SisfR5gVkmVwyCQgCejm-NyALDI4TIZppi1eCa99eof; X-Delve-DigestCookieKor=kyCBqggK33YBHUdlRo9AWbG9SEorCZbhI6jBUj2ozUoBTxI2gMQnbqpLWYRs0Wd0U6qTR7qeAqj_RSvCKqHqZS0GbX0bacHX3z-vYoFrcQk1
referer:https://apc.delve.office.com/?u=4bc41da5-30ca-452c-a6ae-ca57905cbb23&v=work
sec-ch-ua:" Not A;Brand";v="99", "Chromium";v="90", "Google Chrome";v="90"
sec-ch-ua-mobile:?0
sec-fetch-dest:empty
sec-fetch-mode:cors
sec-fetch-site:same-origin
user-agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/90.0.4430.212 Safari/537.36
x-anchormailbox:jie.tian@innocellence.com
x-delve-clientcorrelationid:d44f4ec5-29a8-41a9-ba57-a4c6801a7ba4
x-delve-clientplatform:DelveWeb
x-delve-digest:sFnkIPRFfJ_YEwN8uSU3UY0a4ItxJIyKqdNEvgvrNFkIGBc0nCTHTbpkXtt5NDHL6mwurAtNEaKxFlOpcPw3tJxKQ90nrl0fOc8EfRoOvc6w-gvyS8AltVxoi63_KEuxwZH6GzLy-edWBvLGkXPu0A2
x-delve-flightoverrides:flights='PulseWebExternalContent,PulseWebStoryCards,PulseWebVideoCards,DelveOnOLS,PulseWebFallbackCards,PulseWebContentTypesWave1,PulseWebContentTypeFilter'&overrides=olsmodified,olsrelatedpeople,olsinfeed
x-delve-servercorrelationid:cbfdd33e-65f2-d68f-30dc-351671ea9935 
           */

            List<OrgChart> orgCharts = new List<OrgChart>();

            await GetSubordinate(headUserId, cookies, orgCharts);

            // 完事以后存到数据库里。
            await _orgChartService.AddManyAsync(orgCharts);

            return new JsonResult(new
            {
                Message = $"Success Synced: {orgCharts.Count}"
            });
        }

        private async Task GetSubordinate(string supervisorId, string cookie, List<OrgChart> orgCharts)
        {
            var httpClient = new HttpClient();
            string url = $"https://apc.delve.office.com/mt/v3/people/{supervisorId}/core/directs";
            var message = new HttpRequestMessage(HttpMethod.Get, url);
            message.Headers.Add("Cookie", cookie);
            var result = await httpClient.SendAsync(message);
            var s = await result.Content.ReadAsStringAsync();
            var ocs = JsonConvert.DeserializeObject<OrgChartViewModel>(s);
            orgCharts.AddRange(ocs.d);
            foreach (var oc in ocs.d)
            {
                oc.SupervisorId = supervisorId;
                await GetSubordinate(oc.AadObjectId, cookie, orgCharts);
            }

            return;
        }


        [HttpPost]
        public async Task<IActionResult> GetOrgChart()
        {
            var orgCharts = await _orgChartService.ListAllAsync();

            var nodeDataArray = orgCharts.Select(a =>
               new
               {
                   key = a.AadObjectId,
                   name = a.FullName,
                   title = a.JobTitle,
                   parent = a.SupervisorId,
                   picture = a.ProfileImageAddress
               });

            return new JsonResult(new
            {
                @class = "go.TreeModel",
                nodeDataArray = nodeDataArray
            });
        }
    }
}
