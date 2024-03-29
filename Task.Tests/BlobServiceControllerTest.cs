using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ReenbitTask.Controllers;
using ReenbitTask.Dto;
using ReenbitTask.Services;

namespace Task.Tests
{
    [TestClass]
    public class BlobServiceControllerTest
    {
        [TestMethod]
        public void UploadInvalidExtension()
        {
            //Arrange

            //IFormFile mock
            var content = "Fake text";
            var fileName = "test.txt";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            var file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var fileService = new Mock<IFileService>();

            var blobService = new BlobServiceController(fileService.Object);

            //Act
            Task<IActionResult> result = blobService.Upload(file);

            //Assert
            fileService.Verify(x => x.UploadAsync(It.IsAny<IFormFile>()), Times.Never);

            Assert.AreEqual(400, (result.Result as ObjectResult)?.StatusCode);
        }

        [TestMethod]
        public void UploadValidExtension()
        {
            //Arrange

            //IFormFile mock
            var content = "Fake text";
            var fileName = "test.docx";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;
            var file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var fileService = new Mock<IFileService>();
            var dto = new BlobResponseDto();
            fileService.Setup(x => x.UploadAsync(It.IsAny<IFormFile>())).ReturnsAsync(dto);

            var blobService = new BlobServiceController(fileService.Object);

            //Act
            Task<IActionResult> result = blobService.Upload(file);

            //Assert
            fileService.Verify(x => x.UploadAsync(It.IsAny<IFormFile>()), Times.Once);

            Assert.AreEqual(200, (result.Result as ObjectResult)?.StatusCode);
            
        }
    }
}