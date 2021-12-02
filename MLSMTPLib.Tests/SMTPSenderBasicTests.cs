using System.Collections.Generic;
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
                Address = "no-reply@paniniamerica.net"
            };

            this._mlsmtpRecipient = new MLSMTPRecipient()
            {
                To = new List<string>(){"temp.7179@gmail.com"}
            };

            this._sender = new MLSMTPSender(new MLSMTPSenderConfiguration(), new Logger<MLSMTPSender>(new NullLoggerFactory()));
        }

        [TestMethod]
        public void SendStringMEssageTest()
        {
            _sender.SendMessage(
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
        }

        [TestMethod]
        public void SendHtmlMessage()
        {
            _sender.SendMessage(
                _mlsmtpRecipient,
                new MLSMTPMessage<HTMLContent>()
                {
                    Content = new HTMLContent()
                    {
                        Body = "<h1>Body</h1>",
                        Subject = "Subject"
                    },
                    MessageTemplate = new HTMLMessage()
                },
                _mlsmtpMailFrom
            );
        }
    }
}
