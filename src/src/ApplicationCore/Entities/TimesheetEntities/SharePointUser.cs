using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class SharePointUser : BaseEntity, IAggregateRoot
    {
        public string PersonaTypeString { get; set; }
        public DateTime CreationTimeString { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameFirstLast { get; set; }
        public string DisplayNameLastFirst { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public string EmailAddress { get; set; }
        public string ImAddress { get; set; }
        public string WorkCity { get; set; }
        public int RelevanceScore { get; set; }
        public string ADObjectId { get; set; }
        public string OfficeCountry { get; set; }
        public string OfficeCity { get; set; }

    }
}
