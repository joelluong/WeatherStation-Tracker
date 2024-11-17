namespace API.DTOs;

public class WeatherVariableDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public string LongName { get; set; }
    public List<WeatherDataDto> Data { get; set; }
}