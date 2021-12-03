using System.Collections.Generic;

namespace MaddLogic.MLSMTPLib
{
    public class SMTPSendMessagesSenderStatus
    {
        public List<MessageStatus> messageStatusList { get; set; } = new List<MessageStatus>();
    }
}