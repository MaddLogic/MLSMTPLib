using System.Net.Mail;

namespace MLSMTPLib.Tests
{
    public class MLSMTPSenderConfiguration: IMLSMTPConfiguration
    {
        public string SMTPIP { get; set; } = "smtp.sendgrid.net";
        public int SMTPPort { get; set; } = 587;
        public string SMTPUsername { get; set; } = "apikey";
        public string SMTPPassword { get; set; } = "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";
        public bool EnableSsl { get; set; } = false;
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
    }
}