using System;
using Azure.Storage.Blobs;

namespace AppShop.API.Helper
{
    public class FileStorage : IFileStorage
    {
        private readonly string _connectionString;

        public FileStorage(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureStorage")!;
        }

        public async Task RemoveFileAsync(string path, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync().ConfigureAwait(false);
            var fileName = Path.GetFileName(path);
            var blob = client.GetBlobClient(fileName);
            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
        }

        public async Task<string> SaveFileAsync(byte[] content, string extention, string containerName)
        {
            var client = new BlobContainerClient(_connectionString, containerName);
            await client.CreateIfNotExistsAsync().ConfigureAwait(false);
            client.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.Blob);
            var fileName = $"{Guid.NewGuid()}{extention}";
            var blob = client.GetBlobClient(fileName);

            using(var ms = new MemoryStream(content))
            {
                await blob.UploadAsync(ms).ConfigureAwait(false);
            }

            return blob.Uri.ToString();
        }
    }
}

