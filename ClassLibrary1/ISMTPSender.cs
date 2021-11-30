using System.Collections.Generic;

namespace MLSMTPLib
{
    public interface ISMTPSender
    {
        public MLSMTPSendMessageSenderStatus SendMessage(MLSMTPRecipient recipient, string subject, string message, MLSMTPMailFrom from);

        public MLSMTPSendMessageSenderStatus SendMessage<T>(MLSMTPRecipient recipient, MLSMTPMessage<T> message, MLSMTPMailFrom from) where T : IMessageContent;

        public SMTPSendMessagesSenderStatus SendMessage<T>(IList<MLSMTPRecipient> recepients, MLSMTPMessage<T> message, MLSMTPMailFrom from) where T : IMessageContent;
    }
}