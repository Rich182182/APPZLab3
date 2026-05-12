using System.Linq;
using AutoMapper;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Entities;
using FitnessClub.DAL.Repositories.Interfaces;

namespace FitnessClub.BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AuthService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public UserDto Login(string login, string password)
        {
            var user = _uow.Users.Find(u => u.Login == login && u.Password == password).FirstOrDefault();
            return _mapper.Map<UserDto>(user);
        }

        public bool RegisterClient(string login, string password, string fullName, string phone)
        {
            if (_uow.Users.Find(u => u.Login == login).Any()) return false;

            var client = new Client { FullName = fullName, PhoneNumber = phone };
            _uow.Clients.Create(client);
            _uow.Save();

            var user = new User
            {
                Login = login,
                Password = password,
                Role = "Client",
                ClientId = client.Id
            };
            _uow.Users.Create(user);
            _uow.Save();

            return true;
        }
    }
}