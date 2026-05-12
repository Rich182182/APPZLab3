using System.Collections.Generic;

namespace FitnessClub.DAL.Entities
{
    public class Club : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }

        public ICollection<Training> Trainings { get; set; } = new List<Training>();
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}