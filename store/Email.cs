using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace store
{
    class Email
    {

        public string Subject { get; set; }
        public string[] MailRecipientsTo { get; set; }
        public string[] MailRecipientsCc { get; set; }
        public string Content { get; set; }
        public Attachment Image { get; set; }
        public Attachment File { get; set; }
        public Attachment Vedio { get; set; }

    }
}
