using ReenbitTask.Dto;

namespace ReenbitTask.Services
{
    public interface IFileService
    {
        Task<BlobResponseDto> UploadAsync(IFormFile blob);
    }
}
