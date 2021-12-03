using System;

namespace MaddLogic.MLSMTPLib
{
    public class MessageStatus
    {
        public SMTPRecipient Recipient { get; set; }
        public Guid MessageIdentifier { get; set; }
        public StatusType Type { get; set; }
        public string Message { get; set; }
    }
}