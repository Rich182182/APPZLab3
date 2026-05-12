using FitnessClub.BLL.DTOs;

namespace FitnessClub.BLL.Services.Interfaces
{
    public interface IAuthService
    {
        UserDto Login(string login, string password);
        bool RegisterClient(string login, string password, string fullName, string phone);
    }
}