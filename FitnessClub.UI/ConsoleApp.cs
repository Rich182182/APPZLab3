using System;
using FitnessClub.BLL.DTOs;
using FitnessClub.BLL.Services.Interfaces;

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

                        _admin.CreateSubscriptionType(subName, price, isNetwork);
                        Console.WriteLine("Тип абонемента створено.");
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
                Console.WriteLine("3. Спробувати зайти в клуб");
                Console.WriteLine("0. Вихід з акаунту");
                Console.Write("Вибір: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var clubs = _client.GetClubs();
                        foreach (var cl in clubs)
                        {
                            Console.WriteLine($"[{cl.Id}] {cl.Name} ({cl.Address})");
                        }
                        break;

                    case "2":
                        var types = _client.GetSubscriptionTypes();
                        foreach (var t in types)
                        {
                            string net = t.IsNetworkAccess ? "Мережевий" : "Локальний";
                            Console.WriteLine($"[{t.Id}] {t.Name} - {t.Price} грн ({net})");
                        }

                        Console.Write("Введіть ID типу абонемента: ");
                        if (!int.TryParse(Console.ReadLine(), out int typeId)) break;

                        Console.Write("Введіть ID клубу (0 якщо обрано Мережевий): ");
                        if (!int.TryParse(Console.ReadLine(), out int clubInput)) break;

                        int? clubId = clubInput == 0 ? null : clubInput;

                        bool success = _client.BuySubscription(user.Id, typeId, clubId);
                        Console.WriteLine(success ? "Абонемент успішно придбано." : "Помилка при купівлі.");
                        break;

                    case "3":
                        Console.Write("Введіть ID клубу для відвідування: ");
                        if (!int.TryParse(Console.ReadLine(), out int visitClubId)) break;

                        string result = _client.TryVisitClub(user.Id, visitClubId);
                        Console.WriteLine(result);
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
    }
}