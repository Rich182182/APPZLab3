using System.Collections.Generic;

namespace FitnessClub.DAL.Entities
{
    public class SubscriptionType : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsNetworkAccess { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}