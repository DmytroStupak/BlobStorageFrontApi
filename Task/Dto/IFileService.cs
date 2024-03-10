namespace ReenbitTask.Dto
{
    public interface IFileService
    {
        Task<BlobResponseDto> UploadAsync(IFormFile blob);
    }
}
