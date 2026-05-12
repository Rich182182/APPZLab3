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
    public class FitnessClubService : IFitnessClubService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FitnessClubService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<ClubDto> GetAllClubs()
        {
            var clubs = _unitOfWork.Clubs.GetAll();
            return _mapper.Map<IEnumerable<ClubDto>>(clubs);
        }

        public IEnumerable<SubscriptionTypeDto> GetSubscriptionTypes()
        {
            var types = _unitOfWork.SubscriptionTypes.GetAll();
            return _mapper.Map<IEnumerable<SubscriptionTypeDto>>(types);
        }

        public ClientDto RegisterClient(string fullName, string phoneNumber)
        {
            var existingClient = _unitOfWork.Clients.Find(c => c.PhoneNumber == phoneNumber).FirstOrDefault();

            if (existingClient != null)
            {
                return _mapper.Map<ClientDto>(existingClient);
            }

            var newClient = new Client
            {
                FullName = fullName,
                PhoneNumber = phoneNumber
            };

            _unitOfWork.Clients.Create(newClient);
            _unitOfWork.Save();

            return _mapper.Map<ClientDto>(newClient);
        }

        public bool BuySubscription(int clientId, int subscriptionTypeId, int? clubId, int daysValid)
        {
            var client = _unitOfWork.Clients.Get(clientId);
            var subType = _unitOfWork.SubscriptionTypes.Get(subscriptionTypeId);

            if (client == null || subType == null)
            {
                return false;
            }

            if (!subType.IsNetworkAccess && clubId == null)
            {
                return false;
            }

            var subscription = new Subscription
            {
                ClientId = clientId,
                SubscriptionTypeId = subscriptionTypeId,
                ClubId = subType.IsNetworkAccess ? null : clubId,
                EndDate = DateTime.Now.AddDays(daysValid)
            };

            _unitOfWork.Subscriptions.Create(subscription);
            _unitOfWork.Save();

            return true;
        }

        public bool RegisterVisit(int clientId, int clubId)
        {
            var activeSubscriptions = _unitOfWork.Subscriptions
                .Find(s => s.ClientId == clientId && s.EndDate >= DateTime.Now)
                .ToList();

            if (!activeSubscriptions.Any())
            {
                return false;
            }

            foreach (var sub in activeSubscriptions)
            {
                var subType = _unitOfWork.SubscriptionTypes.Get(sub.SubscriptionTypeId);

                if (subType.IsNetworkAccess)
                {
                    return true;
                }

                if (sub.ClubId == clubId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}