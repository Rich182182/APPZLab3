using FitnessClub.BLL.DTOs;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IClientService
    {
        IEnumerable<ClubDto> GetClubs();
        IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes();
        bool BuySubscription(int userId, int typeId, int? clubId);
        bool BookTraining(int userId, int trainingId);
        string TryVisitClub(int userId, int clubId);
    }
}