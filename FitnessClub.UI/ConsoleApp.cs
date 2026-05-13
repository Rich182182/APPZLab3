using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;
using FitnessClub.DAL.Entities;
using System;

namespace FitnessClub.UI
{
    public class ConsoleApp
    {
        private readonly IAuthService _auth;
        private readonly IAdminService _admin;
        private readonly IClientService _client;

        public ConsoleApp(IAuthService auth, IAdminService admin, IClientService client)
        {
            _auth = auth;
            _admin = admin;
            _client = client;
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                Console.WriteLine("\n--- ГОЛОВНЕ МЕНЮ ---");
                Console.WriteLine("1. Вхід");
                Console.WriteLine("2. Реєстрація");
                Console.WriteLine("0. Вихід");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Login();
                        break;
                    case "2":
                        Register();
                        break;
                    case "0":
                        isRunning = false;
                        break;
                    default:
                        Console.WriteLine("Невідома команда.");
                        break;
                }
            }
        }

        private void Login()
        {
            Console.Write("Логін: ");
            string login = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();

            var user = _auth.Login(login, password);

            if (user == null)
            {
                Console.WriteLine("Помилка авторизації. Перевірте логін або пароль.");
                return;
            }

            switch (user.Role)
            {
                case "Admin":
                    AdminMenu();
                    break;
                default:
                    ClientMenu(user);
                    break;
            }
        }

        private void Register()
        {
            Console.Write("Логін: ");
            string login = Console.ReadLine();
            Console.Write("Пароль: ");
            string password = Console.ReadLine();
            Console.Write("ПІБ: ");
            string name = Console.ReadLine();
            Console.Write("Телефон: ");
            string phone = Console.ReadLine();

            bool success = _auth.RegisterClient(login, password, name, phone);

            if (success)
            {
                Console.WriteLine("Реєстрація успішна. Тепер ви можете увійти.");
            }
            else
            {
                Console.WriteLine("Помилка реєстрації. Можливо, такий логін вже існує.");
            }
        }

        private void AdminMenu()
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
                        Console.Write("Назва клубу: ");
                        string name = Console.ReadLine();
                        Console.Write("Адреса: ");
                        string address = Console.ReadLine();
                        _admin.CreateClub(name, address);
                        Console.WriteLine("Клуб створено.");
                        break;

                    case "2":
                        Console.Write("Назва абонемента: ");
                        string subName = Console.ReadLine();

                        Console.Write("Ціна: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal price))
                        {
                            Console.WriteLine("Невірний формат ціни.");
                            break;
                        }

                        Console.Write("Мережевий доступ (1 - Так, 0 - Ні): ");
                        bool isNetwork = Console.ReadLine() == "1";
                        Console.Write("Час початку (формат: HH): ");
                        if (!int.TryParse(Console.ReadLine(), out int startHour) || startHour < 0 || startHour > 23)
                        {
                            Console.WriteLine("Невірний формат часу початку. Введіть годину (0-23).");
                            break;
                        }
                        TimeSpan startTime = new TimeSpan(startHour, 0, 0);

                        Console.Write("Час закінчення (формат: HH): ");
                        if (!int.TryParse(Console.ReadLine(), out int endHour) || endHour < 0 || endHour > 23)
                        {
                            Console.WriteLine("Невірний формат часу закінчення. Введіть годину (0-23).");
                            break;
                        }
                        TimeSpan endTime = new TimeSpan(endHour, 0, 0);

                        _admin.CreateSubscriptionType(subName, price, isNetwork, startTime, endTime);
                        Console.WriteLine("Тип абонемента створено.");
                        break;

                    case "3":
                        Console.Write("Назва тренування: ");
                        string trainingName = Console.ReadLine();
                        ShowClubs();
                        Console.Write("ID клубу: ");
                        if (!int.TryParse(Console.ReadLine(), out int clubId)) break;
                        Console.Write("Час проведення (формат: yyyy-MM-dd HH:mm): ");
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime scheduleTime)) break;
                        Console.Write("Ціна:");
                        if(!decimal.TryParse(Console.ReadLine(), out decimal trainPrice)) break;
                        _admin.CreateTraining(trainingName, clubId, scheduleTime, trainPrice);
                        Console.WriteLine("Тренування створено.");
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

        private void ClientMenu(UserDto user)
        {
            bool inClientMenu = true;
            while (inClientMenu)
            {
                Console.WriteLine($"\n--- КЛІЄНТ: {user.Login} ---");
                Console.WriteLine("1. Переглянути список клубів");
                Console.WriteLine("2. Придбати абонемент");
                Console.WriteLine("3. Переглянути свої абонементи");
                Console.WriteLine("4. Записатись на тренування");
                Console.WriteLine("5. Відвідати тренування");
                Console.WriteLine($"6. Поповнити рахунок(зараз на рахунку {_client.GetBalance(user.Id)} грн)");
                Console.WriteLine("0. Вихід з акаунту");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowClubs();
                        break;

                    case "2":
                        ShowSubscriptionTypes();

                        Console.Write("Введіть ID типу абонемента: ");
                        if (!int.TryParse(Console.ReadLine(), out int typeId)) break;

                        var subscriptionType = _client.GetSubscriptionTypes().FirstOrDefault(t => t.Id == typeId);
                        if (subscriptionType == null)
                        {
                            Console.WriteLine("Абонемент з таким ID не знайдено.");
                            break;
                        }

                        int? clubId = null;

                        if (!subscriptionType.IsNetworkAccess)
                        {
                            ShowClubs();
                            Console.Write("Введіть ID клубу: ");
                            if (!int.TryParse(Console.ReadLine(), out int clubInput)) break;
                            clubId = clubInput;
                        }

                        bool successBuy = _client.BuySubscription(user.Id, typeId, clubId);
                        Console.WriteLine(successBuy ? "Абонемент успішно придбано." : "Помилка при купівлі.");
                        break;

                    case "3":
                        var subscriptions = _client.GetClientSubscriptions(user.Id);

                        if (!subscriptions.Any())
                        {
                            Console.WriteLine("Вы не имеете активных подписок.");
                            return;
                        }

                        ShowClientSubscriptions(subscriptions);
                        break;

                    case "4":
                        ShowTrainings();

                        Console.Write("Введіть ID тренування для запису: ");
                        if (!int.TryParse(Console.ReadLine(), out int bookTrainingId)) break;

                        bool successBook = _client.BookTraining(user.Id, bookTrainingId);
                        Console.WriteLine(successBook ? "Успішно записано." : "Помилка запису (можливо ви вже записані).");
                        break;

                    case "5":
                        ShowTrainings();
                        Console.Write("Введіть ID тренування, на яке ви прийшли: ");
                        if (!int.TryParse(Console.ReadLine(), out int attendTrainingId)) break;

                        string attendResult = _client.AttendTraining(user.Id, attendTrainingId);
                        Console.WriteLine(attendResult);
                        break;
                    case "6":
                        Console.Write("Введіть суму для поповнення: ");
                        if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                        {
                            Console.WriteLine("Невірний формат суми. Введіть позитивное число.");
                            break;
                        }
                        bool successTopUp = _client.TopUpBalance(user.Id, amount);
                        Console.WriteLine(successTopUp ? "Баланс успішно поповнено." : "Помилка при поповненні.");
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
        public void ShowClubs()
        {
            var clubs = _client.GetClubs();
            foreach (var cl in clubs)
            {
                Console.WriteLine($"[{cl.Id}] {cl.Name} ({cl.Address})");
            }
        }
        public void ShowTrainings()
        {
            var trainings = _client.GetTrainings();
            foreach (var tr in trainings)
            {
                Console.WriteLine($"[{tr.Id}] {tr.Name} (Клуб ID: {tr.ClubId}) - {tr.ScheduleTime}");
            }
        }
        public void ShowClientSubscriptions(IEnumerable<ClientSubscriptionDto> subscriptions)
        {
            Console.WriteLine("\n--- ВАШІ АБОНЕМЕНТИ ---");
            foreach (var sub in subscriptions)
            {
                string status = sub.IsActive ? "✓ Активна" : "✗ Закінчена";
                string network = sub.IsNetworkAccess ? "Мережевий" : $"Локальний ({sub.ClubName})";
                string time = sub.StartTime.HasValue && sub.EndTime.HasValue
                    ? $" ({sub.StartTime:hh\\:mm} - {sub.EndTime:hh\\:mm})"
                    : "";
                Console.WriteLine(status);
                Console.WriteLine($"\n[{sub.Id}] {sub.SubscriptionTypeName} - {network}");
                Console.WriteLine($"  Ціна: {sub.Price} грн |");
                Console.WriteLine($"  дійсний до {sub.EndDate:dd.MM.yyyy}");
                Console.WriteLine($"  {time}");
            }
        }
        public void ShowSubscriptionTypes()
        {
            var types = _client.GetSubscriptionTypes();
            foreach (var t in types)
            {
                string net = t.IsNetworkAccess ? "Мережевий" : "Локальний";
                Console.WriteLine($"[{t.Id}] {t.Name} - {t.Price} грн ({net}) початок: {t.StartTime:hh\\:mm}, кінець: {t.EndTime:hh\\:mm}");
            }
        }
    }
}