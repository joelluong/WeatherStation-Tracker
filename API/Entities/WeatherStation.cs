using CsvHelper.Configuration.Attributes;

namespace API.Entities;

public class WeatherStation
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Site { get; set; }
    public string Portfolio { get; set; }
    public string State { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<WeatherVariable> Variables { get; set; }
}
