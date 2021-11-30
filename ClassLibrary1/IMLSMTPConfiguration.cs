using System.Net.Mail;

namespace MLSMTPLib
{
    public interface IMLSMTPConfiguration
    {
        public string SMTPIP { get; set; }
        int SMTPPort { get; set; }
        SmtpDeliveryMethod DeliveryMethod { get; set; }
    }
}