using FitnessClub.BLL.DTOs;
using FitnessClub.DAL.Entities;
using System.Collections.Generic;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IClientService
    {
        IEnumerable<ClientSubscriptionDto> GetClientSubscriptions(int userId);
        bool BuySubscription(int userId, int typeId, int? clubId);
        bool BookTraining(int userId, int trainingId);
        string AttendTraining(int userId, int trainingId);
        bool TopUpBalance(int userId, decimal amount);
        decimal GetBalance(int userId);
    }
}