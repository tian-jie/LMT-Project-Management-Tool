using System;

namespace BlazorShared.Authorization
{
    public class AspNetMenu
    {
        public AspNetMenu() {
        }

        public string CreatedUserID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsDisplay { get; set; }
        public int? SortCode { get; set; }
        public string NavigateUrl { get; set; }
        public int? MenuType { get; set; }
        public bool? IsDeleted { get; set; }
        public string MenuGroup { get; set; }
        public int? FormID { get; set; }
        public string MenuTitle { get; set; }
        public int? ParentID { get; set; }
        public int? AccountManageId { get; set; }
        public string MenuName { get; set; }
        public int Id { get; set; }
        public string MenuImg { get; set; }
        public int? AppId { get; set; }

    }
}
