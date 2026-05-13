using System;

namespace FitnessClub.BLL.DTOs
{
    public class ClientSubscriptionDto
    {
        public int Id { get; set; }
        public string SubscriptionTypeName { get; set; }
        public decimal Price { get; set; }
        public bool IsNetworkAccess { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public string ClubName { get; set; }
        public bool IsActive => EndDate > DateTime.Now;
    }
}