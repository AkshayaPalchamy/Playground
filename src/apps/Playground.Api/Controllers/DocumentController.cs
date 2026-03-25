using System.Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Playground.Api.Data;
using Playground.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Playground.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "FileUpload")]
    public class DocumentController : ControllerBase
    {
        private readonly string _baseUploadFolder;
        private readonly AppDbContext _context;

        public DocumentController(IWebHostEnvironment env, AppDbContext context)
        {
            _context = context;
            _baseUploadFolder = Path.Combine(env.ContentRootPath, "Upload");

            Directory.CreateDirectory(Path.Combine(_baseUploadFolder, "Admin"));
            Directory.CreateDirectory(Path.Combine(_baseUploadFolder, "Developer"));
        }

        /// UPLOAD - api/file/upload
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
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


            var document = new FileUploadModel
            {
                Role = model.Role,
                Description = model.Description,
                FilePath = filePath,
                UploadOn = DateTime.Now
            };
            _context.Documents.Add(document);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "File uploaded successfully!",
                UserType = model.Role,
                FileName = fileName,
                Description = model.Description
            });
        }

        [HttpGet("{role}")]
        [ApiExplorerSettings(GroupName = "Document")]
        public async Task<IActionResult> GetAllDocuments()
        {
            var documents = await _context.Documents
                .OrderByDescending(d => d.Role)
                .ToListAsync();

            return Ok(documents);
        }
        //DELETE

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletDocument(int id)
        {
            var Document = await _context.Documents.FindAsync(id);

            if(Document == null)
            {
                return NotFound("Document not found");
            }

            if(System.IO.File.Exists(Document.FilePath))
                System.IO.File.Delete(Document.FilePath);
            _context.Documents.Remove(Document);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Document Deleted successfully" });
        }
    }
}
