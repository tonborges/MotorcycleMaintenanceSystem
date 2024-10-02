using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using Application.Features.Motorcycles.Get;
using FluentAssertions;
using Shared.Results;
using Web.Api.ViewModels.Motorcycles;

namespace FunctionalTests.Motorcycles.GetById;

public class GetMotorcycleByIdTest
{
    private readonly HttpClient httpClient = new() { BaseAddress = new Uri(Settings.Url) };

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleByIdThatExists()
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
        var response = await httpClient.GetAsync($"/motos/{viewModel.Identifier}");
        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<GetMotorcycleResponse>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result?.Should().NotBeNull();
        result?.Identifier.Should().Be(viewModel.Identifier);
    }

    [Fact]
    public async Task Handle_ShouldTryGetMotorcycleByIdThatNotExists()
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
        var response = await httpClient.GetAsync("/motos/123");
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        messageResult?.Mensagem.Should().Be("Moto não encontrada");
    }
}