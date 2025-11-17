using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartClinic.Api.Models;
using SmartClinic.Api.Services;

namespace SmartClinic.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Doctor,Receptionist")]
    public class PatientDocumentsController : ControllerBase
    {
        private readonly BlobService _blobService;

        public PatientDocumentsController(BlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadPatientDocument([FromForm] UploadDocumentRequest request)
        {
            // Access the file via the model
            var file = request.File;
            
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var fileUrl = await _blobService.UploadFileAsync(file);
            return Ok(new { FileUrl = fileUrl });
        }
    }
}