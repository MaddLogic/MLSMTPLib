using System.Net.Mail;

namespace MLSMTPLib.Tests
{
    public class MLSMTPSenderConfiguration: IMLSMTPConfiguration
    {
        public string SMTPIP { get; set; } = "123.12.123.10";
        public int SMTPPort { get; set; } = 8080;
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
    }
}