using System;

namespace MLSMTPLib
{
    public class MLSMTPException : Exception
    {
        public MLSMTPException(MLSMTPSendMessageSenderStatus mlsmtpSenderStatus)
        {
        }

        public MLSMTPException(MLSMTPSendMessageSenderStatus mlsmtpSenderStatus, string? message) : base(message)
        {
        }

        public MLSMTPException(MLSMTPSendMessageSenderStatus mlsmtpSenderStatus, string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public MLSMTPSendMessageSenderStatus MlsmtpSenderStatus { get; set; }
    }
}