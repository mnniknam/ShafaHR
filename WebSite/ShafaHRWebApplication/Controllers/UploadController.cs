using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ShafaHRWebApplication.Controllers
{
    [Route("upload")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly string _tempPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads", "Temp");


        [HttpPost("chunk")]
        public async Task<IActionResult> UploadChunk(
            IFormFile file,
            string fileName,
            int chunkIndex,
            int totalChunks,
            long publicationId)
        {
            Directory.CreateDirectory(_tempPath);

            var chunkFilePath = Path.Combine(_tempPath, $"{fileName}.part_{chunkIndex}");

            using (var stream = new FileStream(chunkFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // اگر آخرین chunk بود → merge
            if (chunkIndex == totalChunks - 1)
            {
                await MergeChunks(fileName, totalChunks);
            }

            return Ok();
        }

        private async Task MergeChunks(string fileName, int totalChunks)
        {
            var finalPath = Path.Combine("Uploads", fileName);

            using var finalStream = new FileStream(finalPath, FileMode.Create);

            for (int i = 0; i < totalChunks; i++)
            {
                var partPath = Path.Combine(_tempPath, $"{fileName}.part_{i}");
                using var partStream = new FileStream(partPath, FileMode.Open);
                await partStream.CopyToAsync(finalStream);
                System.IO.File.Delete(partPath);
            }
        }
    }
}
