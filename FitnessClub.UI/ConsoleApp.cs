using System;
using FitnessClub.UI.Menus;

namespace FitnessClub.UI
{
    public class ConsoleApp
    {
        private readonly MainMenu _mainMenu;

        public ConsoleApp(MainMenu mainMenu)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            _mainMenu = mainMenu;
        }

        public void Run()
        {
            _mainMenu.Display();
        }
    }
}