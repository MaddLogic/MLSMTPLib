using System.Net.Mail;

namespace MLSMTPLib
{
    public struct MLSMTPMessage<T> where T:IMessageContent
    {
        public T  Content{ get; set;}
        public IMLSMTPMessageTemplate<T> MessageTemplate { get; set; }

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