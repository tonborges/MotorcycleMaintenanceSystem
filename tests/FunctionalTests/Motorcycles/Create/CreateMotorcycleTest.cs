using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Web.Api.ViewModels.Motorcycles;
using FluentAssertions;
using Shared.Results;

namespace FunctionalTests.Motorcycles.Create;

public class CreateMotorcycleTest
{
    private readonly HttpClient httpClient = new() { BaseAddress = new Uri(Settings.Url) };

    [Fact]
    public async Task Handle_ShouldCreateMotorcycle()
    {
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2024,
            Plate = Guid.NewGuid().ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");

        // Act
        var response = await httpClient.PostAsync("/motos", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Handle_ShouldCreateMotorcycleSamePlate()
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
        viewModel.Identifier = Guid.NewGuid().ToString();
        content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        
        // Act
        var response = await httpClient.PostAsync("/motos", content);
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        messageResult?.Mensagem.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task Handle_ShouldCreateMotorcycleWithInvalidRequest()
    {
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2021
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        
        // Act
        var response = await httpClient.PostAsync("/motos", content);
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        messageResult?.Mensagem.Should().Be("Dados inválidos");
    }
}