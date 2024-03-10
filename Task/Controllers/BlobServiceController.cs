using Microsoft.AspNetCore.Mvc;
using ReenbitTask.Dto;

namespace ReenbitTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobServiceController : Controller
    {
        private readonly IFileService _fileService;

        public BlobServiceController(IFileService fileService)
        {
            _fileService = fileService;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (!IsFileExtensionAllowed(file, ".docx")) 
            {
                return BadRequest("Invalid file type. Please upload DOCX file.");
            }
   
            var result = await _fileService.UploadAsync(file);
            return Ok(result);
        }

        private bool IsFileExtensionAllowed(IFormFile file, string allowedExtensions)
        {
            var extension = Path.GetExtension(file.FileName);
            return allowedExtensions.Contains(extension);
        }
    }
}
