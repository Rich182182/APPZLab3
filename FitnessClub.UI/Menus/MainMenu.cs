using System;
using FitnessClub.BLL.Services.Interfaces;

namespace FitnessClub.UI.Menus
{
    public class MainMenu : IMenu
    {
        private readonly IAuthService _auth;
        private readonly AuthMenu _authMenu;
        private readonly AdminMenu _adminMenu;
        private readonly ClientMenu _clientMenu;

        public MainMenu(IAuthService auth, AuthMenu authMenu, AdminMenu adminMenu, ClientMenu clientMenu)
        {
            _auth = auth;
            _authMenu = authMenu;
            _adminMenu = adminMenu;
            _clientMenu = clientMenu;
        }

        public void Display()
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
                        LoginFlow();
                        break;
                    case "2":
                        _authMenu.Register();
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

        private void LoginFlow()
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
                    _adminMenu.Display();
                    break;
                default:
                    _clientMenu.Display(user);
                    break;
            }
        }
    }
}