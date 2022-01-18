using System.Net.Mail;
using MaddLogic.MLSMTPLib;

namespace MLSMTPLib.Tests
{
    public class SMTPSenderConfiguration: ISMTPConfiguration
    {
        public string SMTPIP { get; set; } = "smtp.mailtrap.io";
        public int SMTPPort { get; set; } = 2525;
        public string SMTPUsername { get; set; } = "0551f8be91cd38";

        public string SMTPPassword { get; set; } = "b05fb884fcd149";
        public bool EnableSsl { get; set; } = true;
        public SmtpDeliveryMethod DeliveryMethod { get; set; } = SmtpDeliveryMethod.Network;
    }
}