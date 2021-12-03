using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using MaddLogic.MLSMTPLib.MailMessages;
using Microsoft.Extensions.Logging;

namespace MaddLogic.MLSMTPLib
{
    public class SMTPSender : ISMTPSender
    {
        private readonly ISMTPConfiguration _configuration;
        private readonly ILogger<SMTPSender> _logger;

        public SMTPSender(ISMTPConfiguration configuration, ILogger<SMTPSender> _logger)
        {
            _configuration = configuration;
            this._logger = _logger;
        }

        public SMTPSendMessageSenderStatus SendMessage(
            SMTPRecipient recipient,
            string subject,
            string message,
            SMTPMailFrom from)
        {
            var msg = new SMTPMessage<SimpleContent>()
            {
                Content = new SimpleContent(),
                MessageTemplate = new StringMessage()
            };

            return new SMTPSendMessageSenderStatus()
            {
                MessageStatus = DoSendMessage(new[] {recipient}, msg, from).messageStatusList.First()
            };
        }

        public SMTPSendMessageSenderStatus SendMessage<T>(SMTPRecipient recipient, SMTPMessage<T> message, SMTPMailFrom @from) where T : IMessageContent
        {
            return new SMTPSendMessageSenderStatus()
            {
                MessageStatus = DoSendMessage(new[] {recipient}, message, from).messageStatusList.First()
            };
        }

        public SMTPSendMessagesSenderStatus SendMessage<T>(IList<SMTPRecipient> recepients, SMTPMessage<T> message, SMTPMailFrom from) where T : IMessageContent
        {
            return DoSendMessage(recepients.ToArray(), message, from);
        }


        private SMTPSendMessagesSenderStatus DoSendMessage<T> (
            SMTPRecipient[] recipients,
            SMTPMessage<T> message,
            SMTPMailFrom from) where T : IMessageContent
        {
            SmtpClient client = new SmtpClient(_configuration.SMTPIP, _configuration.SMTPPort);
            SMTPSendMessagesSenderStatus sendStatus = new SMTPSendMessagesSenderStatus();
            client.DeliveryMethod = _configuration.DeliveryMethod;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_configuration.SMTPUsername, _configuration.SMTPPassword);
            client.EnableSsl = _configuration.EnableSsl;
            foreach (var mlsmtpRecipient in recipients)
            {

                try
                {
                    if (!mlsmtpRecipient.HasRecipient)
                    {
                        continue;
                    }

                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(from.Address, from.Name);

                    foreach (var to in mlsmtpRecipient.To)
                    {
                        mail.To.Add(new MailAddress(to));
                    }

                    foreach (var cc in mlsmtpRecipient.Cc)
                    {
                        mail.CC.Add(new MailAddress(cc));
                    }

                    foreach (var cc in mlsmtpRecipient.Bcc)
                    {
                        mail.Bcc.Add(new MailAddress(cc));
                    }

                    mail.Subject = message.GetSubject();
                    mail.Body = message.GetBody();
                    mail.IsBodyHtml = message.MessageTemplate.IsHtml;

                    if (message.MessageTemplate.CanHaveAttachments)
                    {
                        var attachments = message.GetAttachments();
                        if (attachments != null)
                        {
                            foreach (var attachment in attachments.ToList())
                            {
                                mail.Attachments.Add(attachment);
                            }
                        }
                    }

                    client.Send(mail);

                    sendStatus.messageStatusList.Add(
                        new MessageStatus()
                        {
                            Type = StatusType.GOOD,
                            MessageIdentifier = message.Content.MessageId
                        }
                    );
                }
                catch (Exception e)
                {
                    _logger.Log(LogLevel.Error, e.Message, e);
                    sendStatus.messageStatusList.Add(
                        new MessageStatus()
                        {
                            Type = StatusType.ERROR,
                            MessageIdentifier = message.Content.MessageId
                        }
                    );
                }
            }

            return sendStatus;
        }
    }
}