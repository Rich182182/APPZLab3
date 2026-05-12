using FitnessClub.BLL.DTOs;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IAdminService
    {
        void CreateClub(string name, string address);
        void CreateSubscriptionType(string name, decimal price, bool isNetwork);
        void CreateTraining(string name, int clubId, DateTime time);
        IEnumerable<UserDto> GetAllUsers();
    }
}