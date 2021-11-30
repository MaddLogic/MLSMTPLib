using System.Collections.Generic;

namespace MLSMTPLib
{
    public interface ISMTPSender
    {
        MLSMTPSendMessageSenderStatus SendMessage(MLSMTPRecipient recipient, string subject, string message, MLSMTPMailFrom from);

        MLSMTPSendMessageSenderStatus SendMessage<T>(MLSMTPRecipient recipient, MLSMTPMessage<T> message, MLSMTPMailFrom from) where T : IMessageContent;

        SMTPSendMessagesSenderStatus SendMessage<T>(IList<MLSMTPRecipient> recepients, MLSMTPMessage<T> message, MLSMTPMailFrom from) where T : IMessageContent;
    }
}