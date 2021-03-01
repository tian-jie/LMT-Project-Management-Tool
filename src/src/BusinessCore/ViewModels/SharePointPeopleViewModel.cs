using System;

namespace Microsoft.eShopWeb.BusinessCore.ViewModel
{
    public class SharePointPeopleViewModel
    {
        public string __type { get; set; }
        public Personaid PersonaId { get; set; }
        public string PersonaTypeString { get; set; }
        public DateTime CreationTimeString { get; set; }
        public string DisplayName { get; set; }
        public string DisplayNameFirstLast { get; set; }
        public string DisplayNameLastFirst { get; set; }
        public string FileAs { get; set; }
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public Emailaddress EmailAddress { get; set; }
        public Emailaddress[] EmailAddresses { get; set; }
        public string ImAddress { get; set; }
        public string WorkCity { get; set; }
        public int RelevanceScore { get; set; }
        public AttributionVM[] AttributionsArray { get; set; }
        public OfficeLocation[] OfficeLocationsArray { get; set; }
        public string ADObjectId { get; set; }
    }

    public class Personaid
    {
        public string __type { get; set; }
        public string Id { get; set; }
    }

    public class Emailaddress
    {
        public string Name { get; set; }
        public string EmailAddress { get; set; }
        public string RoutingType { get; set; }
        public string MailboxType { get; set; }
    }

    public class AttributionVM
    {
        public string Id { get; set; }
        public SourceId SourceId { get; set; }
        public string DisplayName { get; set; }
        public bool IsWritable { get; set; }
        public bool IsQuickContact { get; set; }
        public bool IsHidden { get; set; }
        public object FolderId { get; set; }
        public object FolderName { get; set; }
        public bool IsGuest { get; set; }
    }

    public class SourceId
    {
        public string __type { get; set; }
        public string Id { get; set; }
    }

    public class OfficeLocation
    {
        public string Value { get; set; }
        public string[] Attributions { get; set; }
    }

}
