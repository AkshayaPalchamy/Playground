using System.ComponentModel.DataAnnotations;

namespace Playground.Api.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get;set;  }
        public string? Role { get; set; }
        public string? Description { get;set;  }
    }
}
