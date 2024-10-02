using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using FluentAssertions;
using Shared.Results;
using Web.Api.ViewModels.Motorcycles;

namespace FunctionalTests.Motorcycles.UpdatePlate;

public class UpdateMotorcyclePlateTest
{
    private readonly HttpClient httpClient = new() { BaseAddress = new Uri(Settings.Url) };

    [Fact]
    public async Task Handle_ShouldUpdateMotorcyclePlate()
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
        await httpClient.PostAsync("/motos", content);
        
        var updateViewModel = new UpdateMotorcyclePlateViewModel
        {
            Plate = Guid.NewGuid().ToString()
        };
        content = new StringContent(JsonSerializer.Serialize(updateViewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        
        // Act
        var response = await httpClient.PutAsync($"/motos/{viewModel.Identifier}/placa", content);
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        messageResult?.Mensagem.Should().Be("Placa modificada com sucesso");
    }

    [Fact]
    public async Task Handle_ShouldUpdateMotorcyclePlateThatNotExists()
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
        await httpClient.PostAsync("/motos", content);
        
        var updateViewModel = new UpdateMotorcyclePlateViewModel
        {
            Plate = Guid.NewGuid().ToString()
        };
        content = new StringContent(JsonSerializer.Serialize(updateViewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");

        // Act
        var response = await httpClient.PutAsync($"/motos/xyz/placa", content);
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        messageResult?.Mensagem.Should().Be("Dados inválidos");
    }

    [Fact]
    public async Task Handle_ShouldUpdateMotorcyclePlateWithInvalidRequest()
    {
        // Arrange
        // Arrange
        var viewModel = new CreateMotorcycleViewModel
        {
            Identifier = Guid.NewGuid().ToString(),
            Model = Guid.NewGuid().ToString(),
            Year = 2024,
            Plate = Guid.NewGuid().ToString()
        };
        var content = new StringContent(JsonSerializer.Serialize(viewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");
        await httpClient.PostAsync("/motos", content);
        
        var updateViewModel = new UpdateMotorcyclePlateViewModel
        {
            Plate = ""
        };
        content = new StringContent(JsonSerializer.Serialize(updateViewModel, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping }), Encoding.UTF8, "application/json");

        // Act
        var response = await httpClient.PutAsync($"/motos/{viewModel.Identifier}/placa", content);
        var json = await response.Content.ReadAsStringAsync();
        var messageResult = JsonSerializer.Deserialize<MessageResult>(json, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping, PropertyNameCaseInsensitive = true });
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        messageResult?.Mensagem.Should().Be("Dados inválidos");
    }
}