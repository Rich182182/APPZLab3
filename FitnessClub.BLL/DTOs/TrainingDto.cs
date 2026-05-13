using System;

namespace FitnessClub.BLL.DTOs
{
    public class TrainingDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string TrainerName { get; set; }
        public DateTime ScheduleTime { get; set; }
        public int ClubId { get; set; }

    }
}