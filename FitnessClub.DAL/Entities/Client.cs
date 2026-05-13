using System.Collections.Generic;

namespace FitnessClub.DAL.Entities
{
    public class Client : BaseEntity
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Balance { get; set; } = 100;

        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}