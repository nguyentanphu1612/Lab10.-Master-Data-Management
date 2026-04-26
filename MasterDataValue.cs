namespace ASC.Model
{
    public class MasterDataValue : BaseEntity, IAuditTracker
    {
        public int MasterDataKeyId { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
    }
}