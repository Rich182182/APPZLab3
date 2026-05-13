using System.Collections.Generic;
using FitnessClub.BLL.DTOs;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IDisplayService
    {
        IEnumerable<ClubDto> GetClubs();
        IEnumerable<TrainingDto> GetTrainings();
        IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes();
    }
}