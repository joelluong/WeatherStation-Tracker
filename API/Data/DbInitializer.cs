using System.Globalization;
using API.Entities;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        SeedData(scope.ServiceProvider.GetService<WeatherDbContext>());
    }

    private static void SeedData(WeatherDbContext context)
    {
        context.Database.Migrate();
        if (context.WeatherStations.Any())
        {
            Console.WriteLine("Already have data - no need to seed");
            return;
        }
        
        // Load weather stations
        var stations = LoadFromCsv<WeatherStation, WeatherStationMap>("Data/DemoData/weather_stations.csv"); 
        context.WeatherStations.AddRange(stations);

        // Load variables
        var variables = LoadFromCsv<WeatherVariable, WeatherVariableMap>("Data/DemoData/variables.csv"); 
        context.WeatherVariables.AddRange(variables);
        
        
        foreach (var station in stations)
        {
            Console.WriteLine($"Station name: {station.Name}");
        }
        
        foreach (var variable in variables)
        {
            Console.WriteLine($"Station name: {variable.Name}");
        }
        
        // load data for each station
        foreach (var station in stations)
        {
            var datafiles = Directory.GetFiles("Data/DemoData/", $"data_{station.Id}.csv");
            foreach (var file in datafiles)
            {
                var data = LoadWeatherDataFromCsv(file, variables);
                Console.WriteLine("Station ID: "+ station.Id + "Variable ID: "+ data.First().VariableId + ", Timestamp: " +  data.First().Timestamp);
                context.WeatherData.AddRange(data);
            }
        }
        
        context.SaveChanges();
    }
    
    private static IEnumerable<T> LoadFromCsv<T, TMap>(string path) where TMap : ClassMap<T>, new()
    {
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    
        csv.Context.RegisterClassMap<TMap>(); // Register class map for T
        return csv.GetRecords<T>().ToList();
    }
    
    sealed class WeatherStationMap : ClassMap<WeatherStation>
    {
        public WeatherStationMap()
        {
            Map(m => m.Id).Name("id");
            Map(m => m.Name).Name("ws_name");
            Map(m => m.Site).Name("site");
            Map(m => m.Portfolio).Name("portfolio");
            Map(m => m.State).Name("state");
            Map(m => m.Latitude).Name("latitude");
            Map(m => m.Longitude).Name("longitude");
        }
    }
    
    sealed class WeatherVariableMap : ClassMap<WeatherVariable>
    {
        public WeatherVariableMap()
        {
            Map(m => m.Id).Name("var_id");
            Map(m => m.StationId).Name("id");
            Map(m => m.Name).Name("name");
            Map(m => m.Unit).Name("unit");
            Map(m => m.LongName).Name("long_name");
        }
    }
    
    private static IEnumerable<WeatherData> LoadWeatherDataFromCsv(string path, IEnumerable<WeatherVariable> variables)
    {
        var data = new List<WeatherData>();
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

        csv.Read();
        csv.ReadHeader();
        var headers = csv.Context.Reader.HeaderRecord; // Get the column headers

        // Extract station ID from the file name
        var stationId = ExtractStationId(path);

        foreach (var record in csv.GetRecords<dynamic>())
        {
            var timestamp = ParseTimestamp(record.timestamp); // Parse the timestamp

            foreach (var header in headers.Where(h => h != "timestamp"))
            {
                // Find the variable corresponding to this column header
                var variable = variables.FirstOrDefault(v => v.StationId == stationId && v.Name == header);

                if (variable != null)
                {
                    data.Add(new WeatherData
                    {
                        VariableId = variable.Id,
                        Timestamp = timestamp,
                        Value = Convert.ToDouble(csv.GetField(header)) // Dynamic column value
                    });
                }
            }
        }

        return data;
    }

    private static int ExtractStationId(string filePath)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);
        var idPart = fileName.Replace("data_", "");
        return int.Parse(idPart);
    }
    
    private static DateTime ParseTimestamp(string timestamp)
    {
        string[] formats = {
            "dd/MM/yyyy h:mm:ss tt",
            "dd/MM/yyyy H:mm:ss",
            "dd/MM/yyyy h:mm tt",
            "dd/MM/yyyy H:mm",
            "dd/MM/yyyy HH:mm"
        };

        if (DateTime.TryParseExact(timestamp?.Trim(), formats, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var parsedDate))
        {
            // Convert to UTC
            return DateTime.SpecifyKind(parsedDate, DateTimeKind.Local).ToUniversalTime();
        }

        throw new FormatException($"Invalid timestamp format: {timestamp}");
    }
}