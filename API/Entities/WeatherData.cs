namespace API.Entities;

public class WeatherData
{
    public Guid Id { get; set; } = Guid.NewGuid(); // Automatically generate a unique GUID

    public int VariableId { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }

    public WeatherVariable Variable { get; set; }
}