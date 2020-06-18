using AzureServicesSample.Web.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServicesSample.Web.Interfaces
{
    public interface IAzureStorageService
    {
        public Task<string> UploadFile(FileUploadModel File,string StorageConnectionString);
        public Task<string> UploadFile(IFormFile File,string StorageConnectionString);
        public Task<string> UploadFile(FileStream FileStream, string FileName, FileFormat FileFormat, string StorageConnectionString);
    }
}
