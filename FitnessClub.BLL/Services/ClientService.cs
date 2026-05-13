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

        public ClientService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IEnumerable<ClientSubscriptionDto> GetClientSubscriptions(int userId)
        {
            var user = _uow.Users.Get(userId);
            if (user?.ClientId == null) return Enumerable.Empty<ClientSubscriptionDto>();

            var subscriptions = _uow.Subscriptions.Find(s => s.ClientId == user.ClientId.Value).ToList();

            var result = subscriptions.Select(sub =>
            {
                var subType = _uow.SubscriptionTypes.Get(sub.SubscriptionTypeId);
                var club = sub.ClubId.HasValue ? _uow.Clubs.Get(sub.ClubId.Value) : null;

                return new ClientSubscriptionDto
                {
                    Id = sub.Id,
                    SubscriptionTypeName = subType.Name,
                    Price = subType.Price,
                    IsNetworkAccess = subType.IsNetworkAccess,
                    EndDate = sub.EndDate,
                    StartTime = subType.StartTime,
                    EndTime = subType.EndTime,
                    ClubName = club?.Name ?? "Мережевий доступ"
                };
            }).ToList();

            return result;
        }


        public bool BuySubscription(int userId, int typeId, int? clubId)
        {
            var user = _uow.Users.Get(userId);
            var type = _uow.SubscriptionTypes.Get(typeId);
            var client = user.Client;
            
            if (user == null || type == null || (!type.IsNetworkAccess && clubId == null)) return false;
            if (client.Balance >= type.Price)
            {
                _uow.Subscriptions.Create(new Subscription
                {
                    ClientId = user.ClientId.Value,
                    SubscriptionTypeId = typeId,
                    ClubId = type.IsNetworkAccess ? null : clubId,
                    EndDate = DateTime.Now.AddMonths(1)
                });
                client.Balance-=type.Price;
                _uow.Save();

                return true;
            }
            else return false;
        }

        public bool BookTraining(int userId, int trainingId)
        {
            var user = _uow.Users.Get(userId);
            if (user == null || user.ClientId == null) return false;
            var client = _uow.Clients.Get(user.ClientId.Value);
            bool alreadyBooked = _uow.Bookings.Find(b => b.ClientId == user.ClientId.Value && b.TrainingId == trainingId).Any();
            if (alreadyBooked) return false;
            var training = _uow.Trainings.Get(trainingId);
            if (training == null) return false;

            var subscriptions = _uow.Subscriptions.Find(s => s.ClientId == user.ClientId.Value).ToList();
            var validSubscriptions = subscriptions.Where(s => IsSubscriptionValidForTraining(s, training)).ToList();
            if (validSubscriptions?.Any() == true)
            {
                Console.WriteLine("У вас є дійсний абонемент. Ви можете відвідати тренування.");
                _uow.Bookings.Create(new Booking { ClientId = user.ClientId.Value, TrainingId = trainingId });
                _uow.Save();
                return true;
            }
            else
            {
                Console.WriteLine("У вас немає дійсного абонемента для цього тренування.");
                Console.Write("Хочете купити окреме заняття зараз за " + training.Price + " ? (y/n): ");
                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "y":
                        if (client.Balance >= training.Price)
                        {

                            client.Balance -= training.Price;
                            _uow.Save();
                            Console.WriteLine("Окреме заняття куплено. Ви можете відвідати тренування.");
                            _uow.Bookings.Create(new Booking { ClientId = user.ClientId.Value, TrainingId = trainingId });
                            _uow.Save();
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("На рахунку недостатньо коштів.");
                            return false;
                        }
                    case "n":
                        Console.WriteLine("Операція скасована. Ви не зможете відвідати це тренування без дійсного абонемента або окремого заняття.");
                        return false;
                    default:
                        Console.WriteLine("Невірний вибір. Операція скасована.");
                        return false;
                }

            }
        }


        public string AttendTraining(int userId, int trainingId)
        {
            var user = _uow.Users.Get(userId);
            if (user?.ClientId == null) return "Користувача не знайдено.";

            var training = _uow.Trainings.Get(trainingId);
            if (training == null) return "Тренування не знайдено.";

            bool hasBooking = _uow.Bookings.Find(b => b.ClientId == user.ClientId && b.TrainingId == trainingId).Any();
            if (hasBooking) return "Успішно: Ваш попередній запис підтверджено.";

            var subscriptions = _uow.Subscriptions.Find(s => s.ClientId == user.ClientId.Value).ToList();
            var validSubscriptions = subscriptions.Where(s => IsSubscriptionValidForTraining(s, training)).ToList();
            if (validSubscriptions?.Any() == true) return "Успішно: Прохід за абонементом клубу.";
            return "Відмовлено: На це заняття потрібен попередній запис.";
        }
        public bool TopUpBalance(int userId, decimal amount)
        {
            var user = _uow.Users.Get(userId);
            if (user?.ClientId == null) return false;
            var client = _uow.Clients.Get(user.ClientId.Value);
            client.Balance += amount;
            _uow.Save();
            return true;

        }

        public decimal GetBalance(int userId)
        {
            var user = _uow.Users.Get(userId);
            var client = _uow.Clients.Get(user.ClientId.Value);
            return client.Balance;
        }
        private bool IsSubscriptionValidForTraining(Subscription subscription, Training training)
        {
            

            var subType = _uow.SubscriptionTypes.Get(subscription.SubscriptionTypeId);

            if (subType == null)
                return false;


            if (!subType.IsNetworkAccess && subscription.ClubId != training.ClubId)
                return false;

            if (subscription.EndDate <= training.ScheduleTime.Date)
                return false;

            var trainingTime = training.ScheduleTime.TimeOfDay;
            
            if (trainingTime < subType.StartTime || trainingTime > subType.EndTime) return false;

            return true;
        }
    }
}