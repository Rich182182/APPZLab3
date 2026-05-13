using System;
using System.Collections.Generic;
using FitnessClub.BLL.DTOs;

namespace FitnessClub.UI.Presenters
{
    public class ConsolePresenter
    {
        public void ShowClubs(IEnumerable<ClubDto> clubs)
        {
            Console.WriteLine("\n--- СПИСОК КЛУБІВ ---");
            foreach (var club in clubs)
            {
                Console.WriteLine($"[{club.Id}] {club.Name} ({club.Address})");
            }
        }

        public void ShowTrainings(IEnumerable<TrainingDto> trainings)
        {
            Console.WriteLine("\n--- СПИСОК ТРЕНУВАНЬ ---");
            foreach (var training in trainings)
            {
                Console.WriteLine($"[{training.Id}] {training.Name} (Клуб ID: {training.ClubId}) - {training.ScheduleTime}");
            }
        }

        public void ShowSubscriptionTypes(IEnumerable<SubscriptionTypeDto> types)
        {
            Console.WriteLine("\n--- ТИПИ АБОНЕМЕНТІВ ---");
            foreach (var t in types)
            {
                string net = t.IsNetworkAccess ? "Мережевий" : "Локальний";
                Console.WriteLine($"[{t.Id}] {t.Name} - {t.Price} грн ({net}) початок: {t.StartTime:hh\\:mm}, кінець: {t.EndTime:hh\\:mm}");
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
    }
}