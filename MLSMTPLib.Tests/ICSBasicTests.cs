using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Text;
using MaddLogic.MLSMTPLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MLSMTPLib.Tests
{
    [TestClass]
    public class ICSBasicTests
    {
        private ICSEvent _icsEvent;
        
        private SMTPMailFrom _smtpMailFrom;
        private SMTPRecipient _smtpRecipient;

        [TestInitialize]
        public void TestInitialize()
        {
            this._icsEvent = new ICSEvent()
            {
                Description = "Hello World",
                Location = "1000 Broadway Ave., Brooklyn",
                Summary = "TEST SUMMARY",
                BeginDate = "2021-12-13 01:14:12",
                EndDate = "2021-12-14 01:14:12",
                HtmlDescription = "<h1>HELLO WORLD<h1>",
            };
            
            this._smtpMailFrom = new SMTPMailFrom()
            {
                Name = "ICS Test Mailer",
                Address = "from@example.com"
            };

            this._smtpRecipient = new SMTPRecipient()
            {
                To = new List<string>(){"to@example.com"}
            };
        }

        [TestMethod]
        public void TestICSAttachment()
        {
            var SMTPConfig = new SMTPSenderConfiguration();
            var ics = new ICSAttachment();
            var attachment = ics.GenerateICS(this._icsEvent, "Event.ics");
            
            SmtpClient smtpClient = new SmtpClient(SMTPConfig.SMTPIP, SMTPConfig.SMTPPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(SMTPConfig.SMTPUsername, SMTPConfig.SMTPPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Subject = "Test Subject";
            mail.Body = "Test Body";
            //Setting From , To and CC
            mail.From = new MailAddress(this._smtpMailFrom.Address);
            mail.To.Add(new MailAddress(this._smtpRecipient.To[0]));
            mail.Attachments.Add(attachment);
            smtpClient.Send(mail);
            
            Assert.IsInstanceOfType(attachment, typeof(Attachment));
        }
        
        [TestMethod]
        public void TestCalendarICS()
        {
            var SMTPConfig = new SMTPSenderConfiguration();
            
            var body = "Check attachment";
            var subject = "ICS attachment test"; 
            var from = "from@example.com";
            var to = "to@example.com";
            var BeginDate = "2021-12-12 01:14:12";
            var EndDate = "2021-12-13 01:14:12";
            var DetailsHTML = "<h1>HELLO WORLD<h1>";
            var Summary = "TEST SUMMARY";
            var location = "1000 Broadway Ave., Brooklyn";
            
            SmtpClient smtpClient = new SmtpClient(SMTPConfig.SMTPIP, SMTPConfig.SMTPPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(SMTPConfig.SMTPUsername, SMTPConfig.SMTPPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;
            //Setting From , To and CC
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            
            // magic begins
            
             StringBuilder sb = new StringBuilder();
             string DateFormat = "yyyyMMddTHHmmssZ";
             string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
             sb.AppendLine("BEGIN:VCALENDAR");
             sb.AppendLine("VERSION:2.0");
             sb.AppendLine("METHOD:PUBLISH");
            
             DateTime dtStart = Convert.ToDateTime(BeginDate);
             DateTime dtEnd = Convert.ToDateTime(EndDate);
             
             sb.AppendLine("BEGIN:VEVENT");
             sb.AppendLine("SUMMARY:" + Summary);
             sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
             sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
             sb.AppendLine("LOCATION:" + location);
             sb.AppendLine("DESCRIPTION:" + DetailsHTML);
             sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + DetailsHTML);
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
             System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");
            
             mail.Attachments.Add(attachment);

             //magic ends
             
            smtpClient.Send(mail);

        }
        
        [TestMethod]
        public void TestCalendarICSWeblink()
        {
            var SMTPConfig = new SMTPSenderConfiguration();
            
            var body = "Check Attachment";
            var subject = "ICS With Weblink"; 
            var from = "from@example.com";
            var to = "to@example.com";
            
            var BeginDate = "2021-12-12 01:14:12";
            var EndDate = "2021-12-13 01:14:12";
            var DetailsHTML = "<h1>HELLO WORLD<h1> <a href=\"https://maddlogic.com\">Test Maddlogic Site</a>";
            var Summary = "TEST SUMMARY";
            var location = "1000 Broadway Ave., Brooklyn";
            
            SmtpClient smtpClient = new SmtpClient(SMTPConfig.SMTPIP, SMTPConfig.SMTPPort);
            smtpClient.Credentials = new System.Net.NetworkCredential(SMTPConfig.SMTPUsername, SMTPConfig.SMTPPassword);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();
            mail.Subject = subject;
            mail.Body = body;
            //Setting From , To and CC
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            
            // magic begins
            
             StringBuilder sb = new StringBuilder();
             string DateFormat = "yyyyMMddTHHmmssZ";
             string now = DateTime.Now.ToUniversalTime().ToString(DateFormat);
             sb.AppendLine("BEGIN:VCALENDAR");
             sb.AppendLine("VERSION:2.0");
             sb.AppendLine("METHOD:PUBLISH");
            
             DateTime dtStart = Convert.ToDateTime(BeginDate);
             DateTime dtEnd = Convert.ToDateTime(EndDate);
             
             sb.AppendLine("BEGIN:VEVENT");
             sb.AppendLine("SUMMARY:" + Summary);
             sb.AppendLine("DTSTART:" + dtStart.ToUniversalTime().ToString(DateFormat));
             sb.AppendLine("DTEND:" + dtEnd.ToUniversalTime().ToString(DateFormat));
             sb.AppendLine("LOCATION:" + location);
             sb.AppendLine("DESCRIPTION:" + DetailsHTML);
             sb.AppendLine("X-ALT-DESC;FMTTYPE=text/html:" + DetailsHTML);
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
             System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, "event.ics", "text/calendar");
            
             mail.Attachments.Add(attachment);

             //magic ends
             
            smtpClient.Send(mail);

        }
    }
}