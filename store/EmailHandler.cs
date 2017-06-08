using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace store
{
    class EmailHandler
    {

        private static string storagekey = ConfigurationManager.AppSettings["StorageConnectionstring"];

        public static void SendEmail ()
        {
            Email email = new Email();

            email.MailRecipientsTo = new string[] { ConfigurationManager.AppSettings["mailTo"] };
            email.MailRecipientsCc = new string[] { ConfigurationManager.AppSettings["mailTo"] };

            email.Subject = "test email";
            email.Content = "Hi, How are you doing ? " + "<br/><br/>";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storagekey);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("testcontainer");

            #region Image

            CloudBlockBlob blob = container.GetBlockBlobReference("image.jpg");

            var stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            ContentType content = new ContentType(MediaTypeNames.Image.Jpeg);
            email.Image = new Attachment(stream, content);

            #endregion

            #region text file

            blob = container.GetBlockBlobReference("file.txt");

            stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            content = new ContentType(MediaTypeNames.Text.Plain);
            email.File = new Attachment(stream, content);

            #endregion

            #region vedio file

            blob = container.GetBlockBlobReference("vedio.mp4");

            stream = new MemoryStream();
            blob.DownloadToStream(stream);
            stream.Seek(0, SeekOrigin.Begin);
            content = new ContentType("video/mpeg");
            email.Vedio = new Attachment(stream, content);

            #endregion

            SendMail(email);
        }

        private static void SendMail(Email email)
        {
            try
            {
                SmtpClient smtpClient = EmailClientBuilder();
                var emailMessage = MessageBuilder(email);
                smtpClient.Send(emailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static SmtpClient EmailClientBuilder()
        {
            string emailServer = ConfigurationManager.AppSettings["emailServer"];
            int emailPort = Convert.ToInt32(ConfigurationManager.AppSettings["emailPort"]);
            string emailCredentialUserName = ConfigurationManager.AppSettings["emailCredentialUserName"];
            string emailCredentialPassword = ConfigurationManager.AppSettings["emailCredentialPassword"];
            SmtpClient smtpClient = new SmtpClient(emailServer, emailPort);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential(emailCredentialUserName, emailCredentialPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            return smtpClient;
        }

        private static MailMessage MessageBuilder(Email email)
        {
            string fromAddress = ConfigurationManager.AppSettings["fromAddress"];

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromAddress);
            mail.Body = email.Content;
            mail.Subject = email.Subject;
            mail.IsBodyHtml = true;

            if (email.Image != null)
                mail.Attachments.Add(email.Image);
            if (email.File != null)
                mail.Attachments.Add(email.File);
            if (email.Vedio != null)
                mail.Attachments.Add(email.Vedio);

            foreach (var mailRecipient in email.MailRecipientsTo)
            {
                mail.To.Add(new MailAddress(mailRecipient));
            }

            return mail;
        }

    }
}
