namespace Kevin.T.Clockify.Data.Models
{
    public class ClientModel : BaseEntity, IAggregateRoot
    {
        public string id { get; set; }
        public string name { get; set; }

        public string note { get; set; }
    }
}
