namespace Pezza.Api.Helpers
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using ImageMagick;
    using Pezza.Common;
    using Pezza.Common.Models;

    public static class MediaHelper
    {
        public static async Task<Result<UploadMediaResult>> UploadMediaAsync(string uploadFolder, string fileData)
        {
            if (string.IsNullOrEmpty(fileData))
            {
                var folderName = string.IsNullOrEmpty(uploadFolder) ? "" : uploadFolder;

                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Media", folderName));
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                var extension = fileData.GetMimeFromBase64().GetExtensionFromMimeType();
                var timestamp = $"{ DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}";
                var imageFileName = $"{timestamp}{extension}";
                var thumbnailFileName = $"{timestamp}_Thumbnail{extension}";

                var fullPath = Path.Combine(pathToSave, imageFileName);
                var thumbnailFullPath = Path.Combine(pathToSave, thumbnailFileName);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                try
                {
                    var base64Data = Regex.Match(fileData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                    var binData = Convert.FromBase64String(base64Data);

                    await File.WriteAllBytesAsync(fullPath, binData);
                }
                catch (Exception ex)
                {
                    return Result<UploadMediaResult>.Failure(ex.Message);
                }

                CompressImage(fullPath);
                CreateThumbnail(fullPath, thumbnailFullPath);

                var relativeName = Path.Combine(folderName, imageFileName);
                var relativeThumnailName = Path.Combine(folderName, thumbnailFileName);

                return Result<UploadMediaResult>.Success(new UploadMediaResult
                {
                    FullPath = fullPath,
                    RelativePath = relativeName,
                    Thumbnail = relativeThumnailName
                });
            }

            return Result<UploadMediaResult>.Failure("No image to upload");
        }

        private static void CompressImage(string path)
        {
            var newImage = new FileInfo(path);
            var optimizer = new ImageOptimizer();
            optimizer.LosslessCompress(newImage);
            optimizer.Compress(newImage);
            newImage.Refresh();
        }

        private static void CreateThumbnail(string originalPath, string thumbnailPath)
        {
            using var mImage = new MagickImage(originalPath);
            mImage.Sample(new Percentage(10.0));
            mImage.Quality = 82;
            mImage.Density = new Density(72);

            mImage.Write(thumbnailPath);
        }

        public static string GetMimeFromBase64(this string value)
        {
            var match = Regex.Match(value, @"data:(?<type>.+?);base64,(?<data>.+)");
            return match.Groups["type"].Value;
        }
    }    
}
