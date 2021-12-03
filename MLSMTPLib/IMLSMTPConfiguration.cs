using System.Net.Mail;

namespace MLSMTPLib
{
    public interface IMLSMTPConfiguration
    {
        string SMTPIP { get; set; }
        int SMTPPort { get; set; }
        
        string SMTPUsername { get; set; }
        string SMTPPassword { get; set; }
        bool EnableSsl { get; set; }
        
        SmtpDeliveryMethod DeliveryMethod { get; set; }
    }
}