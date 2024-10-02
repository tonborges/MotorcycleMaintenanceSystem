namespace Application.Abstractions.Storage;

public interface IStorageService
{
    Task UploadBase64FileAsync(string? base64String, string? fileName);
    Task<string> DownloadBase64FileAsync(string? fileName);
    Task RemoveFileAsync(string? fileName);
}