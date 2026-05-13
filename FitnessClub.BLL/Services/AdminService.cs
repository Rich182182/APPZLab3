using System;
using System.Collections.Generic;
using AutoMapper;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Entities;
using FitnessClub.DAL.Repositories.Interfaces;

namespace FitnessClub.BLL.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public AdminService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public void CreateClub(string name, string address)
        {
            _uow.Clubs.Create(new Club { Name = name, Address = address });
            _uow.Save();
        }

        public void CreateSubscriptionType(string name, decimal price, bool isNetwork, TimeSpan startTime, TimeSpan endTime)
        {
            _uow.SubscriptionTypes.Create(new SubscriptionType { Name = name, Price = price, IsNetworkAccess = isNetwork, StartTime = startTime, EndTime = endTime });
            _uow.Save();
        }

        public void CreateTraining(string name, int clubId, DateTime time, decimal price)
        {
            _uow.Trainings.Create(new Training { Name = name, ClubId = clubId, ScheduleTime = time, Price=price });
            _uow.Save();
        }

        public IEnumerable<UserDto> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserDto>>(_uow.Users.GetAll());
        }
    }
}