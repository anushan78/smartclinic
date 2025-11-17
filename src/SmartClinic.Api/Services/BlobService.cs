using Azure.Storage.Blobs;

namespace SmartClinic.Api.Services
{
    public class BlobService
    {
        private readonly string _containerName;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(IConfiguration configuration)
        {
            var blobSettings = configuration.GetSection("BlobStorage");
            var connectionString = blobSettings.GetValue<string>("ConnectionString");
            _containerName = blobSettings.GetValue<string>("ContainerName");

            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        public async Task<string> UploadFileAsync(IFormFile file)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = containerClient.GetBlobClient($"{Guid.NewGuid().ToString()}_{file.FileName}");

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            return blobClient.Uri.ToString();
        }
    }
}