using System;

namespace MaddLogic.MLSMTPLib
{
    public class SMTPException : Exception
    {
        public SMTPException(SMTPSendMessageSenderStatus smtpSenderStatus)
        {
        }

        public SMTPException(SMTPSendMessageSenderStatus smtpSenderStatus, string message) : base(message)
        {
        }

        public SMTPException(SMTPSendMessageSenderStatus smtpSenderStatus, string message, Exception innerException = null) : base(message, innerException)
        {
        }

        public SMTPSendMessageSenderStatus SmtpSenderStatus { get; set; }
    }
}