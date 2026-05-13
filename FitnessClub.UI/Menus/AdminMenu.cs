using System;
using System.Linq;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.UI.Presenters;

namespace FitnessClub.UI.Menus
{
    public class AdminMenu : IMenu
    {
        private readonly IAdminService _admin;
        private readonly IDisplayService _display;
        private readonly ConsolePresenter _presenter;

        public AdminMenu(IAdminService admin, IDisplayService display)
        {
            _admin = admin;
            _display = display;
            _presenter = new ConsolePresenter();
        }

        public void Display()
        {
            bool inAdminMenu = true;
            while (inAdminMenu)
            {
                Console.WriteLine("\n--- ПАНЕЛЬ АДМІНІСТРАТОРА ---");
                Console.WriteLine("1. Додати клуб");
                Console.WriteLine("2. Додати тип абонемента");
                Console.WriteLine("3. Додати тренування");
                Console.WriteLine("0. Вихід з акаунту");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddClub();
                        break;

                    case "2":
                        AddSubscriptionType();
                        break;

                    case "3":
                        AddTraining();
                        break;

                    case "0":
                        inAdminMenu = false;
                        break;

                    default:
                        Console.WriteLine("Невідома команда.");
                        break;
                }
            }
        }

        private void AddClub()
        {
            _presenter.ShowClubs(_display.GetClubs());
            Console.WriteLine("Додайте новий клуб:");
            Console.Write("Назва клубу: ");
            string name = Console.ReadLine();
            Console.Write("Адреса: ");
            string address = Console.ReadLine();
            _admin.CreateClub(name, address);
            Console.WriteLine("Клуб створено.");
        }

        private void AddSubscriptionType()
        {
            _presenter.ShowSubscriptionTypes(_display.GetSubscriptionTypes());
            Console.WriteLine("Додайте новий тип абонемента:");
            Console.Write("Назва абонемента: ");
            string subName = Console.ReadLine();

            Console.Write("Ціна: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal price))
            {
                Console.WriteLine("Невірний формат ціни.");
                return;
            }

            Console.Write("Мережевий доступ (1 - Так, 0 - Ні): ");
            bool isNetwork = Console.ReadLine() == "1";

            Console.Write("Час початку (формат: HH): ");
            if (!int.TryParse(Console.ReadLine(), out int startHour) || startHour < 0 || startHour > 23)
            {
                Console.WriteLine("Невірний формат часу початку. Введіть годину (0-23).");
                return;
            }
            TimeSpan startTime = new TimeSpan(startHour, 0, 0);

            Console.Write("Час закінчення (формат: HH): ");
            if (!int.TryParse(Console.ReadLine(), out int endHour) || endHour < 0 || endHour > 23)
            {
                Console.WriteLine("Невірний формат часу закінчення. Введіть годину (0-23).");
                return;
            }
            TimeSpan endTime = new TimeSpan(endHour, 0, 0);

            _admin.CreateSubscriptionType(subName, price, isNetwork, startTime, endTime);
            Console.WriteLine("Тип абонемента створено.");
        }

        private void AddTraining()
        {
            _presenter.ShowTrainings(_display.GetTrainings());
            Console.WriteLine("Додайте нове тренування:");
            Console.Write("Назва тренування: ");
            string trainingName = Console.ReadLine();


            if (string.IsNullOrWhiteSpace(trainingName))
            {
                Console.WriteLine("Назва тренування не може бути пустою.");
                return;
            }

            _presenter.ShowClubs(_display.GetClubs());
            Console.Write("ID клубу: ");
            if (!int.TryParse(Console.ReadLine(), out int clubId))
            {
                Console.WriteLine("Невірний формат ID клубу.");
                return;
            }

            var club = _display.GetClubs().FirstOrDefault(c => c.Id == clubId);
            if (club == null)
            {
                Console.WriteLine("Клуб з таким ID не знайдено.");
                return;
            }

            Console.Write("Час проведення (формат: yyyy-MM-dd HH:mm): ");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime scheduleTime))
            {
                Console.WriteLine("Невірний формат часу.");
                return;
            }

            if (scheduleTime <= DateTime.Now)
            {
                Console.WriteLine("Час проведення повинен бути пізніше за поточний час.");
                return;
            }

            Console.Write("Ціна: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal trainPrice))
            {
                Console.WriteLine("Невірний формат ціни.");
                return;
            }

            try
            {
                _admin.CreateTraining(trainingName, clubId, scheduleTime, trainPrice);
                Console.WriteLine("Тренування створено.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при створенні тренування: {ex.Message}");
            }
        }
    }
}