namespace ASC.Model
{
    public class MasterDataKey : BaseEntity, IAuditTracker
    {
        public string? Key { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}