namespace BookStore.Api.Model;
public record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary, string? apiVersion)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
