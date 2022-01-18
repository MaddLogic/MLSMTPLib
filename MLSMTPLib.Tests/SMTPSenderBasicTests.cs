using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using HtmlAgilityPack;
using MaddLogic.MLSMTPLib;
using MaddLogic.MLSMTPLib.MailMessages;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MLSMTPLib.Tests
{
    [TestClass]
    public class SMTPSenderBasicTests
    {
        private SMTPMailFrom _smtpMailFrom;
        private SMTPRecipient _smtpRecipient;
        private ISMTPSender _sender;

        [TestInitialize]
        public void TestInitialize()
        {
            this._smtpMailFrom = new SMTPMailFrom()
            {
                Name = "Migration Auto Mailer",
                Address = "marjone@maddlogic.com"
            };

            this._smtpRecipient = new SMTPRecipient()
            {
                To = new List<string>(){"marjonerey@gmail.com"}
            };
            
            _sender = new SMTPSender(new SMTPSenderConfiguration(), new Logger<SMTPSender>(new NullLoggerFactory()));
        }

        [TestMethod]
        public void SendStringMessageTest()
        {
            var send = _sender.SendMessage(
                _smtpRecipient,
                new SMTPMessage<SimpleContent>()
                {
                    Content = new SimpleContent()
                    {
                        Body = "Body",
                        Subject = "Subject"
                    },
                    MessageTemplate = new StringMessage()
                },
                _smtpMailFrom
            );
           
            Assert.IsNull(send.MessageStatus.Message);
            Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void SendHtmlMessage()
        {
           var send = _sender.SendMessage(
                _smtpRecipient,
                new SMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = "<h1>Body</h1>" +
                               "<img src=\"https://fakeimg.pl/1280x150/?text=Email Banner\">",
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _smtpMailFrom
            );
           
           Assert.IsNull(send.MessageStatus.Message);
           Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void TestSendInvalidHtml()
        {
            var send = _sender.SendMessage(
                _smtpRecipient,
                new SMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = "<h1>Body</hh1>",
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _smtpMailFrom
            );
            
            Assert.AreEqual(send.MessageStatus.Type, StatusType.ERROR);
        }

        [TestMethod]
        public void TestSendInvalidEmail()
        {
            var recipient = new SMTPRecipient()
            {
                To = new List<string>(){"example@gmailcom"}
            };
            
            var send = _sender.SendMessage(
                recipient,
                new SMTPMessage<SimpleContent>()
                {
                    Content = new SimpleContent()
                    {
                        Body = "Body",
                        Subject = "Subject"
                    },
                    MessageTemplate = new StringMessage()
                },
                _smtpMailFrom
            );
            Assert.AreEqual(send.MessageStatus.Type, StatusType.ERROR);
        }

        [TestMethod]
        public void TestSMTP()
        {
                
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential("apikey", "SG._Y4WN7W4BRtWn_ag1-kNJDA.2XbhTf05Y76uvN7k_LsKx5weLi3dlAWzMQzXdM0D3KQ_");
            // smtpClient.UseDefaultCredentials = true; // uncomment if you don't want to use the network credentials
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = false;
            MailMessage mail = new MailMessage();
            mail.Subject = "Test SMTP";
            mail.Body = "Hello WOrld";
            //Setting From , To and CC
            mail.From = new MailAddress("marjone@maddlogic.com", "Marjone");
            mail.To.Add(new MailAddress("marjonerey@gmail.com"));

            smtpClient.Send(mail);
        }

        [TestMethod]
        public void ValidateHTML()
        {
            string html = "<span>Hello world</sspan>";

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            
            bool error = !doc.ParseErrors.Any();

            Assert.IsFalse(error);
        }

        [TestMethod]
        public void ValidateEmailAddress()
        {
            bool valid = false;
            try
            {
                MailAddress m = new MailAddress("a@example.com");

                valid = true;
            }
            catch (FormatException)
            {
                valid = false;
            }
            
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void TestSendHTMLEmailEventRescheduled()
        {
            var html = "<!DOCTYPE html><html><head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width\" initial-scale=\"1\"><!--[if !mso]> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><![endif]--> <meta name=\"x-apple-disable-message-reformatting\"> <title></title><!--[if mso]> <style>*{font-family: sans-serif !important;}</style><![endif]--> <style>*, *:after, *:before{-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;}*{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}html, body, .document{width: 100% !important; height: 100% !important; margin: 0; padding: 0;}body{-webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale; text-rendering: optimizeLegibility;}div[style*=\"margin: 16px 0\"]{margin: 0 !important;}table, td{mso-table-lspace: 0pt; mso-table-rspace: 0pt;}table{border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;}img{-ms-interpolation-mode: bicubic; max-width: 100%; border: 0;}*[x-apple-data-detectors]{color: inherit !important; text-decoration: none !important;}.x-gmail-data-detectors, .x-gmail-data-detectors *, .aBn{border-bottom: 0 !important; cursor: default !important;}.btn{-webkit-transition: all 200ms ease; transition: all 200ms ease;}.btn:hover{background-color: dodgerblue;}@media screen and (max-width: 750px){.container{width: 100%; margin: auto;}.stack{display: block; width: 100%; max-width: 100%;}}p{color: #666666;}p strong{color: #000000;}</style></head><body><div style=\"display: none; max-height: 0px; overflow: hidden;\"> </div><div style=\"display: none; max-height: 0px; overflow: hidden;\">&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;</div><table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" class=\"document\"> <tr> <td valign=\"top\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"text-align: center\"> <img src=\"https://i.ibb.co/P5QBM5d/vtbanner.png\" alt=\"vtbanner\" border=\"0\"/> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"padding-top: 20px\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"100%\"> <tr> <td> <h2>Hello{registrant_firstname},</h2> <p>This event has been rescheduled!</p><p>Your registration has been transferred to the new date/time. Please update your calendar.</p><p><strong>Event Title:</strong>{event_title}</p><p><strong>Event Date/Time:</strong>{event_datetime}</p><p><strong>Event Duration:</strong>{event_duration}</p><br/> <p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1944e1; border-top: 12px solid #1944e1; border-bottom: 12px solid #1944e1; border-right: 18px solid #1944e1; border-left: 18px solid #1944e1; display: inline-block;\">Update Calendar</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #c94e14; border-top: 12px solid #c94e14; border-bottom: 12px solid #c94e14; border-right: 18px solid #c94e14; border-left: 18px solid #c94e14; display: inline-block;\">Attend Event</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1F7F4C; border-top: 12px solid #1F7F4C; border-bottom: 12px solid #1F7F4C; border-right: 18px solid #1F7F4C; border-left: 18px solid #1F7F4C; display: inline-block;\">Update Registration</a></p><br/> <p>Thank You,</p><p>{event_contact_name}</p><p>{event_contact_email}</p></td></tr></table> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td> </td></tr><tr> <td> <unsubscribe></unsubscribe> </td></tr></table> </td></tr></table></body></html>";
            var send = _sender.SendMessage(
                _smtpRecipient,
                new SMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = html,
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _smtpMailFrom
            );
           
            Assert.IsNull(send.MessageStatus.Message);
            Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void TestSendHTMLEmailEventCancelled()
        {
            var html = "<!DOCTYPE html><html><head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width\" initial-scale=\"1\"><!--[if !mso]> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><![endif]--> <meta name=\"x-apple-disable-message-reformatting\"> <title></title><!--[if mso]> <style>*{font-family: sans-serif !important;}</style><![endif]--> <style>*, *:after, *:before{-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;}*{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}html, body, .document{width: 100% !important; height: 100% !important; margin: 0; padding: 0;}body{-webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale; text-rendering: optimizeLegibility;}div[style*=\"margin: 16px 0\"]{margin: 0 !important;}table, td{mso-table-lspace: 0pt; mso-table-rspace: 0pt;}table{border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;}img{-ms-interpolation-mode: bicubic; max-width: 100%; border: 0;}*[x-apple-data-detectors]{color: inherit !important; text-decoration: none !important;}.x-gmail-data-detectors, .x-gmail-data-detectors *, .aBn{border-bottom: 0 !important; cursor: default !important;}.btn{-webkit-transition: all 200ms ease; transition: all 200ms ease;}.btn:hover{background-color: dodgerblue;}@media screen and (max-width: 750px){.container{width: 100%; margin: auto;}.stack{display: block; width: 100%; max-width: 100%;}}p{color: #666666;}p strong{color: #000000;}</style></head><body><div style=\"display: none; max-height: 0px; overflow: hidden;\"> </div><div style=\"display: none; max-height: 0px; overflow: hidden;\">&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;</div><table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" class=\"document\"> <tr> <td valign=\"top\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"text-align: center\"> <img src=\"https://i.ibb.co/P5QBM5d/vtbanner.png\" alt=\"vtbanner\" border=\"0\"/> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"padding-top: 20px\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"100%\"> <tr> <td> <h2>Hello{registrant_firstname},</h2> <p>The event you registered for has been cancelled.</p><p><strong>Event Title:</strong>{event_title}</p><p><strong>Event Date/Time:</strong>{event_datetime}</p><p><strong>Event Duration:</strong>{event_duration}</p><br/> <p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1944e1; border-top: 12px solid #1944e1; border-bottom: 12px solid #1944e1; border-right: 18px solid #1944e1; border-left: 18px solid #1944e1; display: inline-block;\">Update Calendar</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #c94e14; border-top: 12px solid #c94e14; border-bottom: 12px solid #c94e14; border-right: 18px solid #c94e14; border-left: 18px solid #c94e14; display: inline-block;\">Attend Event</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1F7F4C; border-top: 12px solid #1F7F4C; border-bottom: 12px solid #1F7F4C; border-right: 18px solid #1F7F4C; border-left: 18px solid #1F7F4C; display: inline-block;\">Update Registration</a></p><br/> <p>Thank You,</p><p>{event_contact_name}</p><p>{event_contact_email}</p></td></tr></table> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td> </td></tr><tr> <td> <unsubscribe></unsubscribe> </td></tr></table> </td></tr></table></body></html>";
            var send = _sender.SendMessage(
                _smtpRecipient,
                new SMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = html,
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _smtpMailFrom
            );
           
            Assert.IsNull(send.MessageStatus.Message);
            Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void TestSendHTMLEmailEventReminder()
        {
            var html = "<!DOCTYPE html><html><head> <meta charset=\"UTF-8\"> <meta name=\"viewport\" content=\"width=device-width\" initial-scale=\"1\"><!--[if !mso]> <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><![endif]--> <meta name=\"x-apple-disable-message-reformatting\"> <title></title><!--[if mso]> <style>*{font-family: sans-serif !important;}</style><![endif]--> <style>*, *:after, *:before{-webkit-box-sizing: border-box; -moz-box-sizing: border-box; box-sizing: border-box;}*{-ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%;}html, body, .document{width: 100% !important; height: 100% !important; margin: 0; padding: 0;}body{-webkit-font-smoothing: antialiased; -moz-osx-font-smoothing: grayscale; text-rendering: optimizeLegibility;}div[style*=\"margin: 16px 0\"]{margin: 0 !important;}table, td{mso-table-lspace: 0pt; mso-table-rspace: 0pt;}table{border-spacing: 0; border-collapse: collapse; table-layout: fixed; margin: 0 auto;}img{-ms-interpolation-mode: bicubic; max-width: 100%; border: 0;}*[x-apple-data-detectors]{color: inherit !important; text-decoration: none !important;}.x-gmail-data-detectors, .x-gmail-data-detectors *, .aBn{border-bottom: 0 !important; cursor: default !important;}.btn{-webkit-transition: all 200ms ease; transition: all 200ms ease;}.btn:hover{background-color: dodgerblue;}@media screen and (max-width: 750px){.container{width: 100%; margin: auto;}.stack{display: block; width: 100%; max-width: 100%;}}p{color: #666666;}p strong{color: #000000;}</style></head><body><div style=\"display: none; max-height: 0px; overflow: hidden;\"> </div><div style=\"display: none; max-height: 0px; overflow: hidden;\">&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;&zwnj;&nbsp;</div><table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" class=\"document\"> <tr> <td valign=\"top\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"text-align: center\"> <img src=\"https://i.ibb.co/P5QBM5d/vtbanner.png\" alt=\"vtbanner\" border=\"0\"/> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td style=\"padding-top: 20px\"> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"100%\"> <tr> <td> <h2>Hello{registrant_firstname},</h2> <p>Friendly reminder you've registered to attend this event</p><p><strong>Event Title:</strong>{event_title}</p><p><strong>Event Date/Time:</strong>{event_datetime}</p><p><strong>Event Duration:</strong>{event_duration}</p><br/> <p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1944e1; border-top: 12px solid #1944e1; border-bottom: 12px solid #1944e1; border-right: 18px solid #1944e1; border-left: 18px solid #1944e1; display: inline-block;\">Update Calendar</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #c94e14; border-top: 12px solid #c94e14; border-bottom: 12px solid #c94e14; border-right: 18px solid #c94e14; border-left: 18px solid #c94e14; display: inline-block;\">Attend Event</a></p><p><a href=\"{calendar_link}\" target=\"_blank\" style=\"font-size: 18px; font-family: Helvetica, Arial, sans-serif; color: #ffffff; font-weight: bold; text-decoration: none; border-radius: 5px; background-color: #1F7F4C; border-top: 12px solid #1F7F4C; border-bottom: 12px solid #1F7F4C; border-right: 18px solid #1F7F4C; border-left: 18px solid #1F7F4C; display: inline-block;\">Update Registration</a></p><br/> <p>Thank You,</p><p>{event_contact_name}</p><p>{event_contact_email}</p></td></tr></table> </td></tr></table> <table role=\"presentation\" aria-hidden=\"true\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" width=\"750\" class=\"container\"> <tr> <td> </td></tr><tr> <td> <unsubscribe></unsubscribe> </td></tr></table> </td></tr></table></body></html>";
            var send = _sender.SendMessage(
                _smtpRecipient, 
                new SMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = html,
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _smtpMailFrom
            );
           
            Assert.IsNull(send.MessageStatus.Message);
            Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }
        
        
    }
}
