using System.Net;
using System.Net.Http.Json;
using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using BookStore.Api.Model;
using BookStore.Api;

public class WeatherForecastTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public WeatherForecastTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWeatherForecast_ShouldReturnFiveForecasts()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("/weatherforecast");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();
        forecasts.Should().NotBeNull();
        forecasts.Should().HaveCount(5);

        foreach (var forecast in forecasts!)
        {
            forecast.TemperatureC.Should().BeInRange(-20, 55);
            forecast.Summary.Should().NotBeNullOrWhiteSpace();
            forecast.Date.Should().BeAfter(DateOnly.FromDateTime(DateTime.Now));
        }
    }
}
