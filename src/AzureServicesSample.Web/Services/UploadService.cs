using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureServicesSample.Web.Services
{
    public class UploadService
    {
        public static async Task<string> UploadToBlob(IFormFile file, string storageConnectionString)
        {
            var FileName = (ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName).Trim('"').ToLower();
            string[] splitter = { "\\", "/", "." };
            var splittedFileName = FileName.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var fileExtension = splittedFileName[splittedFileName.Length - 1];
            var fileNameToUpload = $"{DateTime.Now}.{fileExtension}".Replace("/", "-").Replace(":", "-").Replace(" ", "-");
            if (FileName.EndsWith(".jpg") || FileName.EndsWith(".jpeg") || FileName.EndsWith(".png") || FileName.EndsWith(".mp4"))
            {
                if (CloudStorageAccount.TryParse(storageConnectionString, out CloudStorageAccount storageAccount))
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer cloudBlobContainer = Debugger.IsAttached ? cloudBlobClient.GetContainerReference("dev") : cloudBlobClient.GetContainerReference("prod");
                    await cloudBlobContainer.CreateIfNotExistsAsync();
                    CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileNameToUpload);
                    using (var imageStream = file.OpenReadStream())
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(imageStream);
                    }
                    return cloudBlockBlob.Uri.ToString();
                }
                else
                {
                    throw new Exception("Something went wrong");
                }
            }
            else
            {
                throw new Exception("Invalid File");
            }
        }
    }
}
