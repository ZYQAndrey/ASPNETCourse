using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace ASPNETCourse.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Labs()
        {
            ViewBag.Message = "Hand On Labs";

            return View();
        }

        public void LabSource(string filename)
        {
            // Retrieve storage account from connection string.
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString);

            // Create the blob client.
            var blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference("aspnet");

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"path\myfile"))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        public ActionResult Lectures()
        {
            ViewBag.Message = "Lectures";

            return View();
        }

        public ActionResult Books()
        {
            ViewBag.Message = "Books";

            return View();
        }
    }
}