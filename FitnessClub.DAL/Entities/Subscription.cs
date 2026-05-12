using System;

namespace FitnessClub.DAL.Entities
{
    public class Subscription : BaseEntity
    {
        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int SubscriptionTypeId { get; set; }
        public SubscriptionType SubscriptionType { get; set; }

        public DateTime EndDate { get; set; }

        public int? ClubId { get; set; }
        public Club Club { get; set; }
    }
}