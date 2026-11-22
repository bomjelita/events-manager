using System.Text.Json;

namespace EventsManager;

public static class DataManager
{
    private const string DataFile = "events.json";

    public static List<Event> LoadData()
    {
        if (!File.Exists(DataFile))
        {
            return new List<Event>();
        }

        try
        {
            string json = File.ReadAllText(DataFile);
            var events = JsonSerializer.Deserialize<List<Event>>(json);
            return events ?? new List<Event>();
        }
        catch (JsonException)
        {
            Console.WriteLine("Предупреждение: файл данных повреждён. Создан новый список мероприятий.");
            return new List<Event>();
        }
        catch (IOException)
        {
            Console.WriteLine("Ошибка: не удалось прочитать файл данных.");
            return new List<Event>();
        }
    }

    public static void SaveData(List<Event> events)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string json = JsonSerializer.Serialize(events, options);
            File.WriteAllText(DataFile, json);
        }
        catch (IOException)
        {
            Console.WriteLine("Ошибка: не удалось сохранить данные в файл!");
        }
    }
}