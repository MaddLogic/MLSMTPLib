using System;
using System.Net.Mail;

namespace MaddLogic.MLSMTPLib.MailMessages
{
    public class SimpleContent : IMessageContent
    {
        public Guid MessageId { get;} = Guid.NewGuid();
        public AttachmentCollection Attachments { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}