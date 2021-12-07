using System;
using System.Collections.Generic;
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
        public void SendStringMEssageTest()
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
        
        
    }
}
