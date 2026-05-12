using System.Collections.Generic;
using FitnessClub.BLL.DTOs;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IFitnessClubService
    {
        IEnumerable<ClubDto> GetAllClubs();
        IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes();
        ClientDto RegisterClient(string fullName, string phoneNumber);
        bool BuySubscription(int clientId, int subscriptionTypeId, int? clubId, int daysValid);
        bool RegisterVisit(int clientId, int clubId);
    }
}