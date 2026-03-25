using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Playground.Api.Models
{
    public class FileUploadModel
    {
        public IFormFile File { get;set;  }
        public string? Role { get; set; }
        public string? Description { get;set;  } = string.Empty;
        public int id {  get; set; }
        public string FilePath { get;set;  }

        [NotMapped]
        public DateTime UploadOn { get;set;  } = DateTime.Now;
    }
}
