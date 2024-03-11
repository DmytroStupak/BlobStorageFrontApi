using Azure.Storage;
using Azure.Storage.Blobs;
using ReenbitTask.Dto;

namespace ReenbitTask.Services
{
    public class FileService : IFileService
    {
        private string _storageAccount = Const.STORAGE_ACCOUNT;
        private string _key = Const.KEY;
        private readonly BlobContainerClient _filesContainer;

        public FileService()
        {
            var credential = new StorageSharedKeyCredential(_storageAccount, _key);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _filesContainer = blobServiceClient.GetBlobContainerClient($"{Const.CONTAINER}");
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob)
        {
            BlobResponseDto response = new();
            BlobClient client = _filesContainer.GetBlobClient(blob.FileName);

            await using (Stream? data = blob.OpenReadStream())
            {
                await client.UploadAsync(data, true);
            }

            response.Status = $"File \"{blob.FileName}\" Uploaded Successfully";
            response.Error = false;
            response.Blob.Uri = client.Uri.AbsoluteUri;
            response.Blob.Name = client.Name;

            return response;
        }
    }
}
