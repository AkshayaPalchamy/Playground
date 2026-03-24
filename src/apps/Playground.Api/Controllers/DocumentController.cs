using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playground.Api.Models;

namespace Playground.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentController : ControllerBase
    {
        private readonly string _baseUploadFolder;

        public DocumentController(IWebHostEnvironment env)
        {
            _baseUploadFolder = Path.Combine(env.ContentRootPath, "Admin");

            Directory.CreateDirectory(Path.Combine(_baseUploadFolder,"Admin"));
            Directory.CreateDirectory(Path.Combine(_baseUploadFolder,"Developer"));
        }

        /// UPLOAD - api/file/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ApiExplorerSettings(GroupName = "FileUpload")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadModel model)
        {
            if (model.File == null || model.File.Length == 0)
                return BadRequest("No file uploaded.");

            if (model.Role != "Admin" && model.Role != "Developer")
                return BadRequest("UserType must be 'Admin' or 'Developer'.");

            var folderPath = Path.Combine(_baseUploadFolder, model.Role);
            var fileName = $"{Guid.NewGuid()}_{model.File.FileName}";
            var filePath = Path.Combine(folderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            return Ok(new
            {
                Message = "File uploaded successfully!",
                UserType = model.Role,
                FileName = fileName,
                Description = model.Description
            });
        }
    }
}
