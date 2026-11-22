namespace EventsManager;

public static class UIModule
{
    public static void ShowMenu()
    {
        Console.WriteLine("\n=== Учёт внеклассных мероприятий ===");
        Console.WriteLine("1. Добавить новое мероприятие");
        Console.WriteLine("2. Показать все мероприятия");
        Console.WriteLine("3. Найти мероприятие");
        Console.WriteLine("4. Изменить статус мероприятия");
        Console.WriteLine("5. Удалить мероприятие");
        Console.WriteLine("0. Выход");
    }

    public static void Main()
    {
        while (true)
        {
            ShowMenu();
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    AddEventUI();
                    break;
                case "2":
                    ShowAllEventsUI();
                    break;
                case "3":
                    SearchEventsUI();
                    break;
                case "4":
                    ChangeStatusUI();
                    break;
                case "5":
                    DeleteEventUI();
                    break;
                case "0":
                    Console.WriteLine("До свидания!");
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    private static void AddEventUI()
    {
        Console.Write("Введите название мероприятия: ");
        string name = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Введите дату проведения (дд.мм.гггг): ");
        string dateInput = Console.ReadLine()?.Trim() ?? "";
        if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime date))
        {
            Console.WriteLine("Ошибка: неверный формат даты. Используйте формат дд.мм.гггг (например, 15.11.2026).");
            return;
        }

        Console.Write("Введите ответственного учителя: ");
        string responsible = Console.ReadLine()?.Trim() ?? "";

        Console.Write("Введите класс-участник: ");
        string className = Console.ReadLine()?.Trim() ?? "";

        Console.WriteLine($"Допустимые статусы: {string.Join(", ", EventsLogic.ValidStatuses)}");
        Console.Write("Введите статус (по умолчанию 'Запланировано'): ");
        string status = Console.ReadLine()?.Trim() ?? "Запланировано";
        if (string.IsNullOrWhiteSpace(status)) status = "Запланировано";

        try
        {
            EventsLogic.AddEvent(name, date, responsible, className, status);
            Console.WriteLine($"Мероприятие '{name}' добавлено!");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void ShowAllEventsUI()
    {
        var events = EventsLogic.GetAllEvents();
        if (events.Count == 0)
        {
            Console.WriteLine("Список мероприятий пуст.");
            return;
        }

        Console.WriteLine("\nСписок внеклассных мероприятий:");
        foreach (var e in events)
        {
            Console.WriteLine($"  [{e.Date:dd.MM.yyyy}] {e.Name} — Ответственный: {e.Responsible}, Класс: {e.Class}, Статус: {e.Status}");
        }
    }

    private static void SearchEventsUI()
    {
        Console.Write("Введите название, ответственного, класс или дату для поиска: ");
        string query = Console.ReadLine()?.Trim() ?? "";

        var results = EventsLogic.SearchEvents(query);
        if (results.Count == 0)
        {
            Console.WriteLine("Мероприятия не найдены.");
            return;
        }

        Console.WriteLine($"\nНайдено {results.Count} мероприятий:");
        foreach (var e in results)
        {
            Console.WriteLine($"  [{e.Date:dd.MM.yyyy}] {e.Name} — Ответственный: {e.Responsible}, Класс: {e.Class}, Статус: {e.Status}");
        }
    }

    private static void ChangeStatusUI()
    {
        Console.Write("Введите название мероприятия: ");
        string eventName = Console.ReadLine()?.Trim() ?? "";

        Console.WriteLine($"Допустимые статусы: {string.Join(", ", EventsLogic.ValidStatuses)}");
        Console.Write("Введите новый статус: ");
        string newStatus = Console.ReadLine()?.Trim() ?? "";

        try
        {
            if (EventsLogic.ChangeStatus(eventName, newStatus))
            {
                Console.WriteLine($"Статус мероприятия '{eventName}' изменён на '{newStatus}'.");
            }
            else
            {
                Console.WriteLine("Мероприятие с таким названием не найдено.");
            }
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private static void DeleteEventUI()
    {
        Console.Write("Введите название мероприятия для удаления: ");
        string eventName = Console.ReadLine()?.Trim() ?? "";

        EventsLogic.DeleteEvent(eventName);
        Console.WriteLine($"Мероприятие '{eventName}' удалено из списка.");
    }
}