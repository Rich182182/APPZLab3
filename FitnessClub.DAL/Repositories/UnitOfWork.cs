using System;
using FitnessClub.DAL.Context;
using FitnessClub.DAL.Entities;
using FitnessClub.DAL.Repositories.Interfaces;

namespace FitnessClub.DAL.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FitnessContext _context;

        public IRepository<Club> Clubs { get; }
        public IRepository<Client> Clients { get; }
        public IRepository<SubscriptionType> SubscriptionTypes { get; }
        public IRepository<Subscription> Subscriptions { get; }
        public IRepository<Training> Trainings { get; }
        public IRepository<Booking> Bookings { get; }
        public IRepository<User> Users { get; }

#pragma warning disable S107
        public UnitOfWork(
            FitnessContext context,
            IRepository<Club> clubs,
            IRepository<Client> clients,
            IRepository<SubscriptionType> subscriptionTypes,
            IRepository<Subscription> subscriptions,
            IRepository<Training> trainings,
            IRepository<Booking> bookings,
            IRepository<User> users)
#pragma warning restore S107
        {
            _context = context;
            Clubs = clubs;
            Clients = clients;
            SubscriptionTypes = subscriptionTypes;
            Subscriptions = subscriptions;
            Trainings = trainings;
            Bookings = bookings;
            Users = users;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}