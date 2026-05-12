using FitnessClub.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace FitnessClub.DAL.Context
{
    public class FitnessContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<SubscriptionType> SubscriptionTypes { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Training> Trainings { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }

        public FitnessContext(DbContextOptions<FitnessContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subscription>()
                .HasOne(s => s.Club)
                .WithMany(c => c.Subscriptions)
                .HasForeignKey(s => s.ClubId)
                .IsRequired(false);

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Login = "admin",
                Password = "123",
                Role = "Admin"
            });
        }
    }
}