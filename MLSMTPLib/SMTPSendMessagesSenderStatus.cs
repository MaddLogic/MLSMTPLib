using System.Collections.Generic;

namespace MLSMTPLib
{
    public class SMTPSendMessagesSenderStatus
    {
        public List<MessageStatus> messageStatusList { get; set; } = new List<MessageStatus>();
    }
}