using System;
using System.Net.Mail;

namespace MLSMTPLib
{
    public interface IMessageContent
    {
        public Guid MessageId { get;}
        AttachmentCollection Attachments { get; set; }
    }
}