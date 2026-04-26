namespace ASC.Model
{
    public class Product : BaseEntity, IAuditTracker
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? Description { get; set; }
    }
}