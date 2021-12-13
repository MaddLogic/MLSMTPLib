using System.Net.Mail;

namespace MaddLogic.MLSMTPLib
{
    public interface IICSAttachment
    {
        Attachment GenerateICS(ICSEvent Event, string FileName = "event.ics");
    }
}