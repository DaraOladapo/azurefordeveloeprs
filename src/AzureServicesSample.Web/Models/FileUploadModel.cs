using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureServicesSample.Web.Models
{
    public class FileUploadModel
    {
        public string FileName { get; set; }
        public string FileBytes { get; set; }
        public FileFormat FileFormat { get; set; }
    }
    public enum FileFormat
    {
        Image,
        Video,
        Audio
    }
}
