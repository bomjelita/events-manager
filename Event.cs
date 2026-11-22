namespace EventsManager;

public class Event
{
    public string Name { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Responsible { get; set; } = string.Empty;
    public string Class { get; set; } = string.Empty;
    public string Status { get; set; } = "Запланировано";
}