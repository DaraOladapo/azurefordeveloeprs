using AzureServicesSample.Web.Interfaces;
using AzureServicesSample.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AzureServicesSample.Web.Services
{
    public class AzureStorageService : IAzureStorageService
    {
        public async Task<string> UploadFile(IFormFile File, string StorageConnectionString)
        {
            var FileName = (ContentDispositionHeaderValue.Parse(File.ContentDisposition).FileName).Trim('"').ToLower();
            string[] splitter = { "\\", "/", "." };
            var splittedFileName = FileName.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var fileExtension = splittedFileName[splittedFileName.Length - 1];
            var fileNameToUpload = $"{DateTime.Now}.{fileExtension}".Replace("/", "-").Replace(":", "-").Replace(" ", "-");

            if (CloudStorageAccount.TryParse(StorageConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = Debugger.IsAttached ? cloudBlobClient.GetContainerReference("dev") : cloudBlobClient.GetContainerReference("prod");
                await cloudBlobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileNameToUpload);
                using (var FileStream = File.OpenReadStream())
                {
                    await cloudBlockBlob.UploadFromStreamAsync(FileStream);
                }
                return cloudBlockBlob.Uri.ToString();
            }
            else
            {
                throw new Exception("Something went wrong");
            }
        }

        public async Task<string> UploadFile(FileUploadModel File, string StorageConnectionString)
        {

            if (CloudStorageAccount.TryParse(StorageConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(File.FileFormat.ToString().ToLower());
                await cloudBlobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(File.FileName);
                var FileBytes = Convert.FromBase64String(File.FileBytes);
                await cloudBlockBlob.UploadFromByteArrayAsync(FileBytes, 0, FileBytes.Length);
                return cloudBlockBlob.Uri.ToString();
            }
            else
            {
                return "Something went wrong";
            }
        }
        public async Task<string> UploadFile(FileStream FileStream, string FileName, FileFormat FileFormat, string StorageConnectionString)
        {
            if (CloudStorageAccount.TryParse(StorageConnectionString, out CloudStorageAccount storageAccount))
            {
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(FileFormat.ToString());
                await cloudBlobContainer.CreateIfNotExistsAsync();
                CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(FileName);
                await cloudBlockBlob.UploadFromStreamAsync(FileStream);
                return cloudBlockBlob.Uri.ToString();
            }
            else
            {
                return "Something went wrong";
            }
        }
    }
}
