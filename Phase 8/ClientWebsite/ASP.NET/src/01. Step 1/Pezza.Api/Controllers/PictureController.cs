namespace Api.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Common;

    public class PictureController : ApiController
    {
        /// <summary>
        /// Uploads the specified dto.
        /// </summary>
        /// <param name="file">File.</param>
        /// <param name="folder">Folder.</param>
        /// <param name="thumbnail">Return thumbnail or not.</param>
        /// <returns>HttpResponseMessage.</returns>
        /// <response code="200">Picture.</response>
        [HttpGet]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Get(string file, string folder = "", bool thumbnail = false)
        {
            if (!string.IsNullOrEmpty(folder))
            {
                file = @$"{folder}\{file}";
            }

            if (string.IsNullOrEmpty(file))
            {
                return ReturnNotFoundImage();
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Media", file));

            if (thumbnail)
            {
                var extension = Path.GetExtension(path);
                path = path.Replace(extension, extension.Replace(".", "_Thumbnail."));
            }

            if (!System.IO.File.Exists(path))
            {
                return ReturnNotFoundImage();
            }
            else
            {
                path = path.Replace("_Thumbnail", string.Empty);
            }

            if (!System.IO.File.Exists(path))
            {
                return ReturnNotFoundImage();
            }

            var mimetype = MimeTypeMap.GetMimeType(Path.GetExtension(path));

            var stream = new FileStream(path, FileMode.Open);
            return new FileStreamResult(stream, mimetype);
        }

        /// <summary>
        /// Returns the not found image.
        /// </summary>
        /// <returns>Not found image.</returns>
        private static IActionResult ReturnNotFoundImage()
        {
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/not-found.png");
            var stream = new FileStream(imgPath, FileMode.Open);
            return new FileStreamResult(stream, "image/png");
        }
    }
}
