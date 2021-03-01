using System;

namespace Microsoft.eShopWeb.PublicApi.ResourcePlanEndpoints
{
    public class ResourcePlanItem
    {
        public int id { get; set; }
        public string text { get; set; }
        public string ProjectGid { get; set; }
        public string EmployeeGid { get; set; }
        public string TaskName { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string owner_id { get; set; }
        public int duration { get; set; }
        public float progress { get; set; }
        public bool open { get; set; }
        public int Workload { get; set; }
    }
}
