using System;
using FitnessClub.BLL.Services.Interfaces;

namespace FitnessClub.UI.Menus
{
    public class AuthMenu
    {
        private readonly IAuthService _auth;

        public AuthMenu(IAuthService auth)
        {
            _auth = auth;
        }

        public void Register()
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
    }
}