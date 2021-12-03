﻿using System;
using System.Net.Mail;

namespace MaddLogic.MLSMTPLib
{
    public interface IMessageContent
    {
        Guid MessageId { get;}
        AttachmentCollection Attachments { get; set; }

        string Subject { get; set; }
        string Body { get; set; }
    }
}