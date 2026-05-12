using System;
using FitnessClub.DAL.Entities;

namespace FitnessClub.DAL.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Club> Clubs { get; }
        IRepository<Client> Clients { get; }
        IRepository<SubscriptionType> SubscriptionTypes { get; }
        IRepository<Subscription> Subscriptions { get; }
        IRepository<Training> Trainings { get; }
        IRepository<Booking> Bookings { get; }
        IRepository<User> Users { get; }
        void Save();
    }
}