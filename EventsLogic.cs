namespace EventsManager;

public static class EventsLogic
{
    public static readonly string[] ValidStatuses = { "Запланировано", "Проводится", "Завершено", "Отменено" };

    public static void AddEvent(string name, DateTime date, string responsible, string className, string status = "Запланировано")
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Название мероприятия не может быть пустым");
        }

        if (string.IsNullOrWhiteSpace(responsible))
        {
            throw new ArgumentException("Ответственный учитель не может быть пустым");
        }

        if (!ValidStatuses.Contains(status))
        {
            throw new ArgumentException($"Недопустимый статус. Допустимые: {string.Join(", ", ValidStatuses)}");
        }

        var events = DataManager.LoadData();
        var newEvent = new Event
        {
            Name = name.Trim(),
            Date = date,
            Responsible = responsible.Trim(),
            Class = className.Trim(),
            Status = status
        };

        events.Add(newEvent);
        DataManager.SaveData(events);
    }

    public static List<Event> GetAllEvents()
    {
        return DataManager.LoadData();
    }

    public static List<Event> SearchEvents(string query)
    {
        var events = DataManager.LoadData();
        query = query.ToLower();

        return events.Where(e =>
            e.Name.ToLower().Contains(query) ||
            e.Responsible.ToLower().Contains(query) ||
            e.Class.ToLower().Contains(query) ||
            e.Date.ToString("dd.MM.yyyy").Contains(query)
        ).ToList();
    }

    public static bool ChangeStatus(string eventName, string newStatus)
    {
        if (!ValidStatuses.Contains(newStatus))
        {
            throw new ArgumentException($"Недопустимый статус. Допустимые: {string.Join(", ", ValidStatuses)}");
        }

        var events = DataManager.LoadData();
        var targetEvent = events.FirstOrDefault(e => e.Name.ToLower() == eventName.ToLower());

        if (targetEvent != null)
        {
            targetEvent.Status = newStatus;
            DataManager.SaveData(events);
            return true;
        }

        return false;
    }

    public static void DeleteEvent(string eventName)
    {
        var events = DataManager.LoadData();
        events.RemoveAll(e => e.Name.ToLower() == eventName.ToLower());
        DataManager.SaveData(events);
    }
}