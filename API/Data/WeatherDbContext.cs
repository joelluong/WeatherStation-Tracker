using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class WeatherDbContext :DbContext
{
    public DbSet<WeatherStation> WeatherStations { get; set; }
    public DbSet<WeatherVariable> WeatherVariables { get; set; }
    public DbSet<WeatherData> WeatherData { get; set; }

    public WeatherDbContext(DbContextOptions options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherData>()
            .HasKey(d => new { d.Id, d.Timestamp });

        modelBuilder.Entity<WeatherVariable>()
            .HasOne(v => v.Station)
            .WithMany(s => s.Variables)
            .HasForeignKey(v => v.StationId);

        modelBuilder.Entity<WeatherData>()
            .HasOne(d => d.Variable)
            .WithMany(v => v.Data)
            .HasForeignKey(d => d.VariableId);
    }
}