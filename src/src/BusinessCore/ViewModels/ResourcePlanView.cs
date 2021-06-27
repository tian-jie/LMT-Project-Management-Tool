namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    /// <summary>
    /// ��Ŀͳ��
    /// </summary>
    public class ResourcePlanView
    {
        /// <summary>
        /// ����ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Clockify���id�ַ���
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
