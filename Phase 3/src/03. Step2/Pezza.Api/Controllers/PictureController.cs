namespace Pezza.Api.Controllers
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;
    using Pezza.Common;

    public class PictureController : ApiController
    {
        /// <summary>
        /// Uploads the specified dto.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folder"></param>
        /// <param name="thumbnail">Return thumbnail or not.</param>
        /// <returns>HttpResponseMessage.</returns>
        [HttpGet]
        [ProducesResponseType(200)]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Get(string file, string folder = "", bool thumbnail = false)
        {
            if (!string.IsNullOrEmpty(folder))
            {
                file = @$"{folder}\{file}";
            }

            if (string.IsNullOrEmpty(file))
            {
                return this.ReturnNotFoundImage();
            }

            var imageFolder = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Media", file));


            if (thumbnail)
            {
                var extension = Path.GetExtension(imageFolder);
                imageFolder = imageFolder.Replace(extension, extension.Replace(".", "_Thumbnail."));
            }

            if (!System.IO.File.Exists(imageFolder))
            {
                return this.ReturnNotFoundImage();
            }
            else
            {
                imageFolder = imageFolder.Replace("_Thumbnail", "");
            }

            if (!System.IO.File.Exists(imageFolder))
            {
                return this.ReturnNotFoundImage();
            }

            var mimetype = MimeTypeMap.GetMimeType(Path.GetExtension(imageFolder));

            var stream = new FileStream(imageFolder, FileMode.Open);
            return new FileStreamResult(stream, mimetype);
        }

        /// <summary>
        /// Returns the not found image.
        /// </summary>
        /// <returns>Not found image.</returns>
        private IActionResult ReturnNotFoundImage()
        {
            var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/not-found.png");
            var stream = new FileStream(imgPath, FileMode.Open);
            return new FileStreamResult(stream, "image/png");
        }
    }
}
