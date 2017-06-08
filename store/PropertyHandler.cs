using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace store
{
    class PropertyHandler
    {

        private static string storagekey = ConfigurationManager.AppSettings["StorageConnectionstring"];

        private static string containerstring = ConfigurationManager.AppSettings["container"];

        public static void SaveBlobs ()
        {
            var storageAccount = CloudStorageAccount.Parse(storagekey);

            //create blob service client
            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(containerstring);

            //create blob container
            container.CreateIfNotExists();

            container.SetPermissions( new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            #region Save Image to a blob

            string filePath = ConfigurationManager.AppSettings["imagePath"];
            var extension = Path.GetExtension(filePath);
            var filename = "image" + extension;

            //Get a reference to block blob container
            var blockBlob = container.GetBlockBlobReference(filename);

            using (var stream = File.OpenRead(filePath))
            {
                //upload file stream to block blob
                blockBlob.UploadFromStream(stream);
                blockBlob.Properties.ContentType = "image/jpeg";
                blockBlob.SetProperties();
            }

            #endregion

            #region Save text file to a blob

            filePath = ConfigurationManager.AppSettings["filePath"];
            extension = Path.GetExtension(filePath);
            filename = "file" + extension;

            //Get a reference to block blob container
            blockBlob = container.GetBlockBlobReference(filename);
            using (var stream = File.OpenRead(filePath))
            {
                //upload file stream to block blob
                blockBlob.UploadFromStream(stream);
                blockBlob.Properties.ContentType = "text/plain";
                blockBlob.SetProperties();
            }

            #endregion

            #region Save video file to a blob


            filePath = ConfigurationManager.AppSettings["vedioPath"];
            extension = Path.GetExtension(filePath);
            filename = "vedio" + extension;

            //Get a reference to block blob container
            blockBlob = container.GetBlockBlobReference(filename);
            using (var stream = File.OpenRead(filePath))
            {
                //upload file stream to block blob
                blockBlob.UploadFromStream(stream);
                blockBlob.Properties.ContentType = "video/mpeg";
                blockBlob.SetProperties();
            }

            #endregion

        }
    }
}
