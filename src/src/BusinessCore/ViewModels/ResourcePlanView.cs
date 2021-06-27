namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    /// <summary>
    /// 项目统计
    /// </summary>
    public class ResourcePlanView
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clockify里的id字符串
        /// </summary>
        public virtual string ProjectGid { get; set; }

        /// <summary>
        /// EmployeeGid
        /// </summary>
        public string EmployeeGid { get; set; }

        /// <summary>
        /// Year
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Week
        /// </summary>
        public int Week { get; set; }

        /// <summary>
        /// Amount
        /// </summary>
        public float Amount { get; set; }
    }
}
