using System;
using System.Collections.Generic;

namespace FitnessClub.DAL.Entities
{
    public class Training : BaseEntity
    {
        public string Name { get; set; }
        public string TrainerName { get; set; }
        public DateTime ScheduleTime { get; set; }

        public int ClubId { get; set; }
        public Club Club { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}