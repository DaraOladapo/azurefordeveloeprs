using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureServicesSample.Web.Models;
using AzureServicesSample.Web.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AzureServicesSample.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : Controller
    {
        readonly string _StorageConnectionString;
        public FileController(IConfiguration Configuration)
        {
            //_StorageConnectionString = Configuration["ConnectionStrings:AzureBlobConnectionString"];
            _StorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=azdevtraining;AccountKey=a3fW4H9J64NN9VWPv7vfcbeSawm79YsjpwVTYKsRUshWnd4awN+NfHahdNcrr1PA/HgLPHTCSo+E6qV9vMbLOQ==;EndpointSuffix=core.windows.net";
        }
        [HttpPost("Upload")]
        public async Task<ActionResult<string>> Upload([FromBody]FileUploadModel uploadModel)
        {
            var FileUploadResult = await new AzureStorageService().UploadFile(uploadModel, _StorageConnectionString);
            return Ok(FileUploadResult);
        }
    }
}
