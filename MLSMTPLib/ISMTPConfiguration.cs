using System.Net.Mail;

namespace MaddLogic.MLSMTPLib
{
    public interface ISMTPConfiguration
    {
        string SMTPIP { get; set; }
        int SMTPPort { get; set; }
        
        string SMTPUsername { get; set; }
        string SMTPPassword { get; set; }
        bool EnableSsl { get; set; }
        
        SmtpDeliveryMethod DeliveryMethod { get; set; }
    }
}