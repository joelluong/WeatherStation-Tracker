namespace API.DTOs;

public class WeatherStationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Site { get; set; }
    public string Portfolio { get; set; }
    public string State { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public List<WeatherVariableDto> Variables { get; set; }
}