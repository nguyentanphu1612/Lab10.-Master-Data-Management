using System;

namespace ASC.Model
{
    public class ServiceRequest : BaseEntity, IAuditTracker
    {
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? EngineerId { get; set; }
        public string? EngineerName { get; set; }
        public string? ServiceType { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
        public string? Notes { get; set; }
    }
}