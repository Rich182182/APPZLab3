using System;
using System.Linq;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.UI.Presenters;

namespace FitnessClub.UI.Menus
{
    public class ClientMenu : IMenu
    {
        private readonly IClientService _client;
        private readonly IDisplayService _display;
        private readonly ConsolePresenter _presenter;
        private UserDto _currentUser;

        public ClientMenu(IClientService client, IDisplayService display)
        {
            _client = client;
            _display = display;
            _presenter = new ConsolePresenter();
        }

        public void Display()
        {
            throw new NotImplementedException();
        }

        public void Display(UserDto user)
        {
            _currentUser = user;
            bool inClientMenu = true;
            while (inClientMenu)
            {
                Console.WriteLine($"\n--- КЛІЄНТ: {user.Login} ---");
                Console.WriteLine("1. Переглянути список клубів");
                Console.WriteLine("2. Придбати абонемент");
                Console.WriteLine("3. Переглянути свої абонементи");
                Console.WriteLine("4. Записатись на тренування");
                Console.WriteLine("5. Відвідати тренування");
                Console.WriteLine($"6. Поповнити рахунок (зараз на рахунку {_client.GetBalance(user.Id)} грн)");
                Console.WriteLine("0. Вихід з акаунту");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewClubs();
                        break;

                    case "2":
                        BuySubscription();
                        break;

                    case "3":
                        ViewSubscriptions();
                        break;

                    case "4":
                        BookTraining();
                        break;

                    case "5":
                        AttendTraining();
                        break;

                    case "6":
                        TopUpBalance();
                        break;

                    case "0":
                        inClientMenu = false;
                        break;

                    default:
                        Console.WriteLine("Невідома команда.");
                        break;
                }
            }
        }

        private void ViewClubs()
        {
            _presenter.ShowClubs(_display.GetClubs());
        }

        private void ViewSubscriptions()
        {
            var subscriptions = _client.GetClientSubscriptions(_currentUser.Id);

            if (!subscriptions.Any())
            {
                Console.WriteLine("Ви не маєте активних підписок.");
                return;
            }

            _presenter.ShowClientSubscriptions(subscriptions);
        }

        private void BuySubscription()
        {
            var types = _display.GetSubscriptionTypes();
            _presenter.ShowSubscriptionTypes(types);

            Console.Write("Введіть ID типу абонемента: ");
            if (!int.TryParse(Console.ReadLine(), out int typeId)) return;

            var subscriptionType = types.FirstOrDefault(t => t.Id == typeId);
            if (subscriptionType == null)
            {
                Console.WriteLine("Абонемент з таким ID не знайдено.");
                return;
            }

            int? clubId = null;

            if (!subscriptionType.IsNetworkAccess)
            {
                ViewClubs();
                Console.Write("Введіть ID клубу: ");
                if (!int.TryParse(Console.ReadLine(), out int clubInput)) return;
                clubId = clubInput;
            }

            bool successBuy = _client.BuySubscription(_currentUser.Id, typeId, clubId);
            Console.WriteLine(successBuy ? "Абонемент успішно придбано." : "Помилка при купівлі.");
        }

        private void BookTraining()
        {
            _presenter.ShowTrainings(_display.GetTrainings());

            Console.Write("Введіть ID тренування для запису: ");
            if (!int.TryParse(Console.ReadLine(), out int bookTrainingId)) return;

            bool successBook = _client.BookTraining(_currentUser.Id, bookTrainingId);
            Console.WriteLine(successBook ? "Успішно записано." : "Помилка запису (можливо ви вже записані).");
        }

        private void AttendTraining()
        {
            _presenter.ShowTrainings(_display.GetTrainings());

            Console.Write("Введіть ID тренування, на яке ви прийшли: ");
            if (!int.TryParse(Console.ReadLine(), out int attendTrainingId)) return;

            string attendResult = _client.AttendTraining(_currentUser.Id, attendTrainingId);
            Console.WriteLine(attendResult);
        }

        private void TopUpBalance()
        {
            Console.Write("Введіть суму для поповнення: ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
            {
                Console.WriteLine("Невірний формат суми. Введіть позитивне число.");
                return;
            }

            bool successTopUp = _client.TopUpBalance(_currentUser.Id, amount);
            Console.WriteLine(successTopUp ? "Баланс успішно поповнено." : "Помилка при поповненні.");
        }
    }
}