using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using HtmlAgilityPack;
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

        private bool IsValidEmailAdress(string email)
        {
            if (email.Trim().EndsWith(".")) {
                return false; 
            }
            try {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch {
                return false;
            }
        }

        private bool isValidHTML(string content)
        {

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(content);

            return doc.ParseErrors.Count() > 0;
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
                        if (!IsValidEmailAdress(to)) continue;
                        mail.To.Add(new MailAddress(to));
                    }

                    foreach (var cc in mlsmtpRecipient.Cc)
                    {
                        if (!IsValidEmailAdress(cc)) continue;
                        mail.CC.Add(new MailAddress(cc));
                    }

                    foreach (var cc in mlsmtpRecipient.Bcc)
                    {
                        if (!IsValidEmailAdress(cc)) continue;
                        mail.Bcc.Add(new MailAddress(cc));
                    }

                    var body = message.GetBody();
                    var isHTML = message.MessageTemplate.IsHtml;

                    if (isHTML && !isValidHTML(body))
                    {
                        throw new Exception("Invalid HTML body.");
                    }
                    
                    mail.Subject = message.GetSubject();
                    mail.Body = body;
                    mail.IsBodyHtml = isHTML;

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
                            MessageIdentifier = message.Content.MessageId,
                            Message = e.Message
                        }
                    );
                }
            }

            return sendStatus;
        }
    }
}