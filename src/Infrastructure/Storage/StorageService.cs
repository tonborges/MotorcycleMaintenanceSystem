using Application.Abstractions.Storage;
using Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class StorageService : IStorageService
{
    private readonly MinioClient minioClient;
    private readonly string bucketName;
    private readonly ILogger<StorageService> logger;

    public StorageService(
        IOptions<MinioSettings> options,
        ILogger<StorageService> logger)
    {
        minioClient = new MinioClient();
        minioClient
            .WithEndpoint(options.Value.Endpoint, options.Value.Port ?? 9000)
            .WithCredentials(options.Value.AccessKey, options.Value.SecretKey)
            .Build();
        bucketName = options.Value.BucketName ?? "motorcycle-maintenance-system";
        this.logger = logger;
    }

    public async Task UploadBase64FileAsync(string? base64String, string? fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            logger.LogInformation("Checking if bucket exists");
            var found = await minioClient
                              .BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName))
                              .ConfigureAwait(false);
            if (!found)
                await minioClient
                      .MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName))
                      .ConfigureAwait(false);

            var contentType = base64String!.Contains("image/png", StringComparison.InvariantCultureIgnoreCase) ? "image/png" : "image/bmp";
            logger.LogInformation("Converting base64 to bytes");
            var fileBytes = Convert.FromBase64String(base64String.Replace("data:image/png;base64,", string.Empty).Replace("data:image/bmp;base64,", string.Empty));

            logger.LogInformation("Creating temp file");
            var tempFile = Path.GetTempFileName();
            await File.WriteAllBytesAsync(tempFile, fileBytes);

            logger.LogInformation("Uploading file to Minio");
            await minioClient.PutObjectAsync(new PutObjectArgs()
                                             .WithBucket(bucketName)
                                             .WithObject(fileName)
                                             .WithFileName(tempFile)
                                             .WithContentType(contentType))
                             .ConfigureAwait(false);

            logger.LogInformation("Removing temp file");
            File.Delete(tempFile);

            logger.LogInformation($"File '{fileName}' uploaded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error to upload file '{fileName}'");
        }
    }

    public async Task<string> DownloadBase64FileAsync(string? fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return string.Empty;

            logger.LogInformation("Downloading file from storage");
            var tempFile = Path.GetTempFileName();
            await minioClient.GetObjectAsync(new GetObjectArgs()
                                             .WithBucket(bucketName)
                                             .WithObject(fileName)
                                             .WithCallbackStream(stream =>
                                             {
                                                 using (var fileStream = File.Create(tempFile))
                                                 {
                                                     stream.CopyTo(fileStream);
                                                 }
                                             }))
                             .ConfigureAwait(false);

            logger.LogInformation("Converting file to base64");
            var fileBytes = await File.ReadAllBytesAsync(tempFile);
            var base64String = Convert.ToBase64String(fileBytes);

            logger.LogInformation("Removing temp file");
            File.Delete(tempFile);

            return base64String;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error to download file '{fileName}'");
            return string.Empty;
        }
    }

    public async Task RemoveFileAsync(string? fileName)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return;

            logger.LogInformation($"Removing file '{fileName}'");
            await minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                                                .WithBucket(bucketName)
                                                .WithObject(fileName))
                             .ConfigureAwait(false);

            logger.LogInformation($"File '{fileName}' removed successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Erro ao remover o arquivo '{fileName}'");
        }
    }
}