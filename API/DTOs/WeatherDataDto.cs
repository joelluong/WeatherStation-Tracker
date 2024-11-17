namespace API.DTOs;

public class WeatherDataDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Value { get; set; }
}