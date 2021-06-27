using Kevin.T.Clockify.Data.Models;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.BusinessCore.ServiceInterfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Microsoft.eShopWeb.BusinessCore.Services
{
    public class ClockifyService : IClockifyService
    {
        private readonly IAppLogger<ClockifyService> _logger;
        private object _locker = new object();


        public ClockifyService(IAppLogger<ClockifyService> logger)
        {
            _logger = logger;
        }


        public async Task<LoginModel> Login(string username, string password)
        {
            string url = "https://global.api.clockify.me/auth/token";
            string loginBodyFormat = "{{\"email\":\"{0}\",\"password\":\"{1}\"}}";

            string loginBody = string.Format(loginBodyFormat, username, password);
            var content = new StringContent(loginBody, System.Text.Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/login");
            //httpClient.DefaultRequestHeaders.Add(":authority", "global.api.clockify.me");

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await httpClient.PostAsync(url, content);


            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginModel>(res);

            return resultModel;
        }

        public async Task<List<UserGroupModel>> GetUserGroups(string userid, string token)
        {
            string url = string.Format("https://global.api.clockify.me/workspaces/{0}/userGroups/", userid);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            //httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("global.api.clockify.me");
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/reports/detailed");

            var response = await httpClient.GetAsync(url);


            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserGroupModel>>(res);

            return resultModel;
        }

        public async Task<List<UserModel>> GetUsers(string userid, string token)
        {
            string url = string.Format("https://global.api.clockify.me/workspaces/{0}/users", userid);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/reports/detailed");

            var response = await httpClient.GetAsync(url);

            var res = await response.Content.ReadAsStringAsync();

            _logger.LogInformation(res);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserModel>>(res);

            return resultModel;
        }
        public async Task<List<TimeEntryModel>> GetTimeEntries(string userid, string token, DateTime startDate, DateTime endDate)
        {
            string url = string.Format("https://global.api.clockify.me/workspaces/{0}/reports/summary/entries", userid);

            string bodyFormat = "{{\"userGroupIds\":[],\"userIds\":[],\"projectIds\":[],\"clientIds\":[],\"taskIds\":[],\"tagIds\":[],\"billable\":\"BOTH\",\"description\":\"\",\"firstTime\":true,\"archived\":\"All\"," +
                "\"startDate\":\"{0}\",\"endDate\":\"{1}\"," +
                "\"me\":\"TEAM\",\"includeTimeEntries\":true,\"zoomLevel\":\"month\",\"name\":\"\",\"groupingOn\":false,\"groupedByDate\":false,\"page\":0,\"sortDetailedBy\":\"timeAsc\",\"count\":1000000}}";

            string body = string.Format(bodyFormat, startDate.ToString("s") + "Z", endDate.AddDays(1).ToString("s") + "Z");

            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/reports/detailed");

            httpClient.Timeout = new TimeSpan(0, 10, 0);

            var response = await httpClient.PostAsync(url, content);

            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TimeEntryModel>>(res);

            return resultModel;
        }


        public async Task<List<TimeEntryModelV2>> GetTimeEntriesV2(string userid, string token, DateTime startDate, DateTime endDate)
        {
            List<TimeEntryModelV2> timeEntryModelV2s = new List<TimeEntryModelV2>();

            var page = 1;
            var r = await GetTimeEntriesByPageV2(userid, token, page, startDate, endDate);

            timeEntryModelV2s.AddRange(r.timeEntries);
            var totalRecord = r.totals[0].entriesCount;
            _logger.LogInformation($"an item queried: result.timeEntries = {r.timeEntries.Count}");

            if (totalRecord <= 200)
            {
                return timeEntryModelV2s;
            }
            var tasks = new List<Task>();

            // 给参数放到列表里，给下面的队列抢
            ConcurrentQueue<QueueObject> queue = new ConcurrentQueue<QueueObject>();
            for (var i = 2; i < totalRecord / 200 + 2; i++)
            {
                queue.Enqueue(new QueueObject()
                {
                    UserId = userid,
                    Token = token,
                    PageId = i,
                    StartDate = startDate,
                    EndDate = endDate
                });
                _logger.LogInformation($"new queue added: page = {i}");
            }

            // 然后task分页，或者抢占式，8线程
            for (var i = 0; i < 16; i++)
            {
                _logger.LogInformation($"new task created: taskid = {i}");
                tasks.Add(Task.Run(async () =>
                {
                    QueueObject item = null;
                    while (queue.TryDequeue(out item))
                    {
                        try
                        {
                            _logger.LogInformation($"an item dequeued: page = {item.PageId}");
                            // 取出来一个item就开始处理，取不出来就结束
                            var result = await GetTimeEntriesByPageV2(item.UserId, item.Token, item.PageId, item.StartDate, item.EndDate);

                            lock (_locker)
                            {
                                _logger.LogInformation($"an item queried: result.timeEntries = {result.timeEntries.Count}");
                                timeEntryModelV2s.AddRange(result.timeEntries);
                            }
                        }
                        catch (Exception ex)
                        {
                            // 如果出错了，给东西放回去
                            _logger.LogInformation("Task failed, put item to queue again.");
                            _logger.LogInformation(ex.Message);
                            queue.Enqueue(item);
                        }
                    }
                }));
            }
            Task.WaitAll(tasks.ToArray());
            //do
            //{
            //    page++;
            //    r = await GetTimeEntriesByPageV2(userid, token, page, startDate, endDate);

            //    timeEntryModelV2s.AddRange(r.timeEntries);
            //    totalRecord = r.totals[0].entriesCount;
            //} while (page * 200 < totalRecord);

            return timeEntryModelV2s;
        }


        public async Task<List<TimeEntry>> GetTimeEntriesV3(string userid, string token, DateTime startDate, DateTime endDate)
        {
            var url = "https://reports.api.clockify.me/workspaces/5d5f3b1bbf6ed132e4c82eb8/reports/summary";
            List<TimeEntry> timeEnteries = new List<TimeEntry>();

            var date = startDate;
            while (date <= endDate)
            {
                var dateString = date.ToString("yyyy-MM-dd");

                string bodyFormat = "{{\"dateRangeStart\":\"{0}T00:00:00.000Z\",\"dateRangeEnd\":\"{0}T23:59:59.999Z\",\"sortOrder\":\"ASCENDING\",\"description\":\"\",\"rounding\":false,\"withoutDescription\":false,\"amountShown\":\"HIDE_AMOUNT\",\"zoomLevel\":\"WEEK\",\"userLocale\":\"zh_CN\",\"customFields\":null,\"summaryFilter\":{{\"sortColumn\":\"GROUP\",\"groups\":[\"USER\",\"PROJECT\",\"TASK\"]}}}}";
                string body = string.Format(bodyFormat, dateString);
                var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");

                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
                httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
                httpClient.DefaultRequestHeaders.Referrer = new System.Uri(url);

                httpClient.Timeout = new TimeSpan(0, 10, 0);

                var response = await httpClient.PostAsync(url, content);

                var res = await response.Content.ReadAsStringAsync();
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    throw new System.Exception(res);
                }

                var resultModel = JsonConvert.DeserializeObject<TimeEntryResponseModelV3>(res);

                var now = DateTime.Now;

                foreach (var u in resultModel.groupOne)
                {
                    foreach (var p in u.children)
                    {
                        foreach (var t in p.children)
                        {

                            timeEnteries.Add(new TimeEntry()
                            {
                                Date = date,
                                Gid = "",
                                IsBillable = true,
                                IsDeleted = false,
                                IsLocked = false,
                                ProjectId = p._id,
                                TaskId = t._id,
                                TotalHours = (decimal)(t.duration / 3600f),
                                UserId = u._id,
                                CreatedDate = now,
                                CreatedUserID = "",
                                CreatedUserName = "",
                                UpdatedDate = now,
                                UpdatedUserID = "",
                                UpdatedUserName = ""
                            });
                        }
                    }
                }
                _logger.LogInformation(JsonConvert.SerializeObject(timeEnteries));
                date = date.AddDays(1);

            }

            return timeEnteries;
        }


        public async Task<TimeEntryResponseModelV2> GetTimeEntriesByPageV2(string userid, string token, int pageId, DateTime startDate, DateTime endDate)
        {
            string urlTemplate = "https://reports.api.clockify.me/workspaces/{0}/reports/detailed";

            string bodyFormat = "{{\"dateRangeStart\":\"{0}T00:00:00.000Z\",\"dateRangeEnd\":\"{1}T23:59:59.999Z\",\"sortOrder\":\"DESCENDING\",\"description\":\"\",\"rounding\":false,\"withoutDescription\":false,\"amountShown\":\"HIDE_AMOUNT\",\"zoomLevel\":\"WEEK\",\"userLocale\":\"zh_CN\",\"customFields\":null,\"detailedFilter\":{{\"sortColumn\":\"DATE\",\"page\":{2},\"pageSize\":200,\"auditFilter\":null}}}}";
            string startString = startDate.ToString("yyyy-MM-dd");
            string endString = endDate.ToString("yyyy-MM-dd");
            string body = string.Format(bodyFormat, startString, endString, pageId);
            _logger.LogInformation(body);

            var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
            var url = string.Format(urlTemplate, userid, startString, endString, pageId);
            var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            //httpClient.DefaultRequestHeaders.Referrer = new System.Uri(url);

            httpClient.Timeout = new TimeSpan(0, 0, 30);

            var response = await httpClient.PostAsync(url, content);

            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            _logger.LogInformation(res);

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeEntryResponseModelV2>(res);

            return resultModel;
        }

        //public async Task<TimeEntryResponseModelV2> GetTimeEntriesByPageV3(string userid, string token, DateTime startDate, DateTime endDate)
        //{
        //    string urlTemplate = "https://reports.api.clockify.me/workspaces/{0}/reports/detailed?start={1}&end={2}&page={3}&pageSize=200";

        //    string bodyFormat = "{{\"dateRangeStart\":\"{0}\",\"dateRangeEnd\":\"{1}\",\"sortOrder\":\"DESCENDING\",\"description\":\"\",\"rounding\":false,\"withoutDescription\":false,\"amountShown\":\"HIDE_AMOUNT\",\"zoomLevel\":\"WEEK\",\"userLocale\":\"zh_CN\",\"customFields\":null,\"detailedFilter\":{{\"sortColumn\":\"ID\",\"page\":{2},\"pageSize\":200,\"auditFilter\":null}}}}";
        //    string startString = startDate.ToString("s") + "Z";
        //    string endString = endDate.AddDays(1).ToString("s") + "Z";
        //    string body = string.Format(bodyFormat, startString, endString, pageId);
        //    _logger.Debug(body);

        //    var content = new StringContent(body, System.Text.Encoding.UTF8, "application/json");
        //    var url = string.Format(urlTemplate, userid, startString, endString, pageId);
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
        //    httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
        //    httpClient.DefaultRequestHeaders.Referrer = new System.Uri(url);

        //    httpClient.Timeout = new TimeSpan(0, 10, 0);

        //    var response = await httpClient.PostAsync(url, content);

        //    var res = await response.Content.ReadAsStringAsync();
        //    if (response.StatusCode != System.Net.HttpStatusCode.OK)
        //    {
        //        throw new System.Exception(res);
        //    }

        //    _logger.Debug(res);

        //    var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<TimeEntryResponseModelV2>(res);

        //    return resultModel;
        //}

        public async Task<List<ClientModel>> GetClients(string userid, string token)
        {
            string url = string.Format("https://global.api.clockify.me/workspaces/{0}/clients", userid);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/reports/detailed");

            var response = await httpClient.GetAsync(url);

            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ClientModel>>(res);

            return resultModel;
        }

        public async Task<List<ProjectModel>> GetProjects(string userid, string token)
        {
            string url = string.Format("https://global.api.clockify.me/workspaces/{0}/projects/reportFilter", userid);

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Origin", "https://clockify.me");
            httpClient.DefaultRequestHeaders.Add("x-auth-token", token);
            httpClient.DefaultRequestHeaders.Referrer = new System.Uri("https://clockify.me/reports/detailed");

            var response = await httpClient.GetAsync(url);

            var res = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new System.Exception(res);
            }

            var resultModel = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ProjectModel>>(res);

            return resultModel;
        }

    }

    class QueueObject
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public int PageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
