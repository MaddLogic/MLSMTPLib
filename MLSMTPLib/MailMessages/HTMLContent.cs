using System;
using System.Net.Mail;

namespace MLSMTPLib
{
    public class HTMLContent: IMessageContent
    {
        public Guid MessageId { get; } = new Guid();
        public AttachmentCollection Attachments { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}