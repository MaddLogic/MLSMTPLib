using System;

namespace MLSMTPLib
{
    public class MessageStatus
    {
        public MLSMTPRecipient Recipient { get; set; }
        public Guid MessageIdentifier { get; set; }
        public StatusType Type { get; set; }
        public string Message { get; set; }
    }
}