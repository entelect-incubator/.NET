namespace Pezza.Api.Helpers;

using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ImageMagick;
using Pezza.Common.Models;

public static class MediaHelper
{
    public static string GetMimeFromBase64(this string value)
    {
        var match = Regex.Match(value, @"data:(?<type>.+?);base64,(?<data>.+)");
        return match.Groups["type"].Value;
    }

    public static Stream GetStreamFromUrl(string url)
    {
        byte[] imageData = null;

        using (var wc = new System.Net.WebClient())
        {
            imageData = wc.DownloadData(url);
        }

        return new MemoryStream(imageData);
    }

    public static async Task<Result<UploadMediaResult>> UploadMediaAsync(string uploadFolder, string base64FileData, bool thumbnail = false)
    {
        if (!string.IsNullOrEmpty(base64FileData))
        {
            var folderName = string.IsNullOrEmpty(uploadFolder) ? string.Empty : uploadFolder;

            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("Media", folderName));
            if (!Directory.Exists(pathToSave))
            {
                Directory.CreateDirectory(pathToSave);
            }

            var extension = base64FileData.GetMimeFromBase64().GetExtensionFromMimeType();
            var timestamp = $"{DateTime.Now:yyyy-MM-dd_HH-mm-ss-fff}";
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
                var base64Data = Regex.Match(base64FileData, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                var binData = Convert.FromBase64String(base64Data);

                await File.WriteAllBytesAsync(fullPath, binData);
            }
            catch (Exception ex)
            {
                return Result<UploadMediaResult>.Failure(ex.Message);
            }

            if (thumbnail)
            {
                using var stream = File.Open(fullPath, FileMode.Open);
                CreateThumbnail(stream, thumbnailFullPath);
            }

            return Result<UploadMediaResult>.Success(new UploadMediaResult
            {
                FullPath = fullPath,
                Path = imageFileName,
                Thumbnail = thumbnailFileName
            });
        }

        return Result<UploadMediaResult>.Failure("No image to upload");
    }

    private static void CreateThumbnail(Stream stream, string thumbnailPath)
    {
        using var mImage = new MagickImage(stream);
        mImage.Sample(new Percentage(10.0));
        mImage.Quality = 60;
        mImage.Density = new Density(60);

        mImage.Write(thumbnailPath);
    }
}