using FitnessClub.BLL.Mapping;
using FitnessClub.BLL.Services;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Context;
using FitnessClub.DAL.Repositories;
using FitnessClub.DAL.Repositories.Interfaces;
using FitnessClub.UI.Menus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FitnessClub.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var services = new ServiceCollection();

            services.AddLogging();
            services.AddDbContext<FitnessContext>(options =>
                options.UseSqlServer("Server=DESKTOP-M42AT8C;Database=FitnessClubDb;Trusted_Connection=True;TrustServerCertificate=True;"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddAutoMapper(cfg => cfg.AddProfile<FitnessMappingProfile>());

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IDisplayService, DisplayService>();

            services.AddTransient<AuthMenu>();
            services.AddTransient<AdminMenu>();
            services.AddTransient<ClientMenu>();
            services.AddTransient<MainMenu>();
            services.AddTransient<ConsoleApp>();

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ConsoleApp>().Run();
        }
    }
}