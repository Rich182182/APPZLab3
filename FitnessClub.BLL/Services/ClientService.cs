using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Entities;
using FitnessClub.DAL.Repositories.Interfaces;

namespace FitnessClub.BLL.Services
{
    public class ClientService : IClientService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ClientService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public IEnumerable<ClubDto> GetClubs() => _mapper.Map<IEnumerable<ClubDto>>(_uow.Clubs.GetAll());

        public IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes() => _mapper.Map<IEnumerable<SubscriptionTypeDto>>(_uow.SubscriptionTypes.GetAll());

        public bool BuySubscription(int userId, int typeId, int? clubId)
        {
            var user = _uow.Users.Get(userId);
            var type = _uow.SubscriptionTypes.Get(typeId);
            if (user == null || type == null || (!type.IsNetworkAccess && clubId == null)) return false;

            _uow.Subscriptions.Create(new Subscription
            {
                ClientId = user.ClientId.Value,
                SubscriptionTypeId = typeId,
                ClubId = type.IsNetworkAccess ? null : clubId,
                EndDate = DateTime.Now.AddMonths(1)
            });
            _uow.Save();
            return true;
        }

        public bool BookTraining(int userId, int trainingId)
        {
            var user = _uow.Users.Get(userId);
            if (user == null || user.ClientId == null) return false;

            _uow.Bookings.Create(new Booking { ClientId = user.ClientId.Value, TrainingId = trainingId });
            _uow.Save();
            return true;
        }

        public string TryVisitClub(int userId, int clubId)
        {
            var user = _uow.Users.Get(userId);
            if (user?.ClientId == null) return "Користувача не знайдено.";

            var activeSubs = _uow.Subscriptions.Find(s => s.ClientId == user.ClientId && s.EndDate > DateTime.Now).ToList();
            if (!activeSubs.Any()) return "Відмовлено: немає активного абонемента.";

            foreach (var sub in activeSubs)
            {
                var type = _uow.SubscriptionTypes.Get(sub.SubscriptionTypeId);
                if (type.IsNetworkAccess) return "Успішно: Мережевий доступ.";
                if (sub.ClubId == clubId) return "Успішно: Домашній клуб.";
            }

            var hasBooking = _uow.Bookings.Find(b => b.ClientId == user.ClientId && _uow.Trainings.Get(b.TrainingId).ClubId == clubId).Any();
            if (hasBooking) return "Успішно: Запис на заняття підтверджено.";

            return "Відмовлено: Ви не записані на заняття у цьому клубі.";
        }
    }
}