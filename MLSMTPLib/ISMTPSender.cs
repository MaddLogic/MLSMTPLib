using System.Collections.Generic;

namespace MaddLogic.MLSMTPLib
{
    public interface ISMTPSender
    {
        SMTPSendMessageSenderStatus SendMessage(SMTPRecipient recipient, string subject, string message, SMTPMailFrom from);

        SMTPSendMessageSenderStatus SendMessage<T>(SMTPRecipient recipient, SMTPMessage<T> message, SMTPMailFrom from) where T : IMessageContent;

        SMTPSendMessagesSenderStatus SendMessage<T>(IList<SMTPRecipient> recepients, SMTPMessage<T> message, SMTPMailFrom from) where T : IMessageContent;
    }
}