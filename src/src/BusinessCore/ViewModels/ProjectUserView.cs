namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    /// <summary>
    /// ��Ŀͳ��
    /// </summary>
    public class ProjectUserView
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
        /// Clockify���id�ַ���
        /// </summary>
        public virtual string UserGid { get; set; }

        /// <summary>
        /// Ա������
        /// </summary>
        public virtual string EmployeeName { get; set; }

        /// <summary>
        /// �û���ɫID
        /// </summary>
        public int UserRoleTitleId { get; set; }

        /// <summary>
        /// �û���ɫ
        /// </summary>
        public string UserRoleTitle { get; set; }

        /// <summary>
        /// Rate
        /// </summary>
        public decimal Rate { get; set; }

        //public IViewModel ConvertAPIModel(object model)
        //{
        //    var entity = (ProjectUser)model;
        //    Id = entity.Id;
        //    ProjectGid = entity.ProjectGid;
        //    UserGid = entity.UserGid;
        //    EmployeeName = entity.EmployeeName;
        //    UserRoleTitleId = entity.UserRoleTitleId;
        //    UserRoleTitle = entity.UserRoleTitle;
        //    Rate = entity.Rate;

        //    return this;
        //}
    }
}
