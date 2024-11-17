using CsvHelper.Configuration.Attributes;

namespace API.Entities;

public class WeatherVariable
{
    public int Id { get; set; }

    public int StationId { get; set; }

    public string Name { get; set; }

    public string Unit { get; set; }

    public string LongName { get; set; }

    public WeatherStation Station { get; set; }
    public ICollection<WeatherData> Data { get; set; }
}