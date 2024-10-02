using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Features.Motorcycles.Get;
using FluentAssertions;
using Shared.Results;
using Web.Api.ViewModels.Motorcycles;

namespace FunctionalTests.Motorcycles.Get;

public class GetMotorcycleTest
{
    private readonly HttpClient httpClient = new() { BaseAddress = new Uri(Settings.Url) };

    [Fact]
    public async Task Handle_ShouldGetMotorcycle()
    {
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2021,
            Plate = Guid.NewGuid().ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/motos", content);

        // Act
        var response = await httpClient.GetAsync("/motos");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetMotorcycleResponse[]>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result?.Length.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleThatExists()
    {
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2021,
            Plate = Guid.NewGuid().ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/motos", content);

        // Act
        var response = await httpClient.GetAsync($"/motos?placa={viewModel.Plate}");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetMotorcycleResponse[]>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result?.Length.Should().BeGreaterOrEqualTo(1);
    }

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleThatNotExists()
    {
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2021,
            Plate = Guid.NewGuid().ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/motos", content);

        // Act
        var response = await httpClient.GetAsync("/motos?placa=xxx");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetMotorcycleResponse[]>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result?.Length.Should().Be(0);
    }
}