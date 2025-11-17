using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartClinic.Api.Models
{
    public class UploadDocumentRequest
    {
        public IFormFile File { get; set; }
    }
}