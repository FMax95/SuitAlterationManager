using System;

namespace SuitAlterationManager.Api.Client.SystemManagement.Responses
{
    public class AlterationResponse
    {
        public Guid Id { get; set; }
        public string CustomerEmail { get; set; }
        public string Type { get; set; }
        public string Direction { get; set; }
        public string Status { get; set; }
        public int MeasureCM { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}