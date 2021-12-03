using System.Collections.Generic;
using System.Net.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MLSMTPLib.Tests
{
    [TestClass]
    public class SMTPSenderBasicTests
    {
        private MLSMTPMailFrom _mlsmtpMailFrom;
        private MLSMTPRecipient _mlsmtpRecipient;
        private ISMTPSender _sender;

        [TestInitialize]
        public void TestInitialize()
        {
            this._mlsmtpMailFrom = new MLSMTPMailFrom()
            {
                Name = "Migration Auto Mailer",
                Address = "marjone@maddlogic.com"
            };

            this._mlsmtpRecipient = new MLSMTPRecipient()
            {
                To = new List<string>(){"marjonerey@gmail.com"}
            };
            
            _sender = new MLSMTPSender(new MLSMTPSenderConfiguration(), new Logger<MLSMTPSender>(new NullLoggerFactory()));
        }

        [TestMethod]
        public void SendStringMEssageTest()
        {
            var send = _sender.SendMessage(
                _mlsmtpRecipient,
                new MLSMTPMessage<SimpleContent>()
                {
                    Content = new SimpleContent()
                    {
                        Body = "Body",
                        Subject = "Subject"
                    },
                    MessageTemplate = new StringMessage()
                },
                _mlsmtpMailFrom
            );
           
            Assert.IsNull(send.MessageStatus.Message);
            Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void SendHtmlMessage()
        {
           var send = _sender.SendMessage(
                _mlsmtpRecipient,
                new MLSMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = "<h1>Body</h1>" +
                               "<img src=\"https://fakeimg.pl/1280x150/?text=Email Banner\">",
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _mlsmtpMailFrom
            );
           
           Assert.IsNull(send.MessageStatus.Message);
           Assert.AreEqual(send.MessageStatus.Type, StatusType.GOOD);
        }

        [TestMethod]
        public void TestSMTP()
        {
                
            SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential("apikey", "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");
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
    }
}
