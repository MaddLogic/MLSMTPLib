using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace MaddLogic.MLSMTPLib
{
    public class ICSAttachment : IICSAttachment
    {
        public Attachment GenerateICS(ICSEvent Event, string FileName)
        {
            StringBuilder sb = new StringBuilder();
            string DateFormat = "yyyyMMddTHHmmssZ";
            string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("METHOD:PUBLISH");
            
            DateTime dtStart = Convert.ToDateTime(Event.BeginDate);
            DateTime dtEnd = Convert.ToDateTime(Event.EndDate);
            
            sb.AppendLine("BEGIN:VEVENT");
            sb.AppendLine("SUMMARY:" + Event.Summary);
            sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
            sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
            sb.AppendLine("LOCATION:" + Event.Location);
            sb.AppendLine("DESCRIPTION:" + Event.Description);
            sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + Event.HtmlDescription);
            sb.AppendLine("STATUS:CONFIRMED");
            sb.AppendLine("SEQUENCE:0");
            sb.AppendLine("DTSTAMP:" + now);
            sb.AppendLine("UID:" + Guid.NewGuid());
            sb.AppendLine("CREATED:" + now);
            sb.AppendLine("LAST-MODIFIED:" + now);
            sb.AppendLine("TRANSP:OPAQUE");
            sb.AppendLine("END:VEVENT");
            
            sb.AppendLine("END:VCALENDAR");
            
            var calendarBytes = Encoding.UTF8.GetBytes(sb.ToString());
            MemoryStream ms = new MemoryStream(calendarBytes);
            
            return new System.Net.Mail.Attachment(ms, FileName, "text/calendar");
        }
    }
}