using System.Net.Mail;

namespace MaddLogic.MLSMTPLib
{
    public struct SMTPMessage<T> where T:IMessageContent
    {
        public T  Content{ get; set;}
        public ISMTPMessageTemplate<T> MessageTemplate { get; set; }

        public string GetSubject()
        {
            return MessageTemplate.GetSubject(Content);
        }

        public string GetBody()
        {
            return MessageTemplate.GetBody(Content);
        }

        public AttachmentCollection GetAttachments()
        {
            return Content.Attachments;
        }
    }
}