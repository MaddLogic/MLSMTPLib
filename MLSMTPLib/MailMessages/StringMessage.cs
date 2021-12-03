namespace MaddLogic.MLSMTPLib.MailMessages
{
    public class StringMessage: ISMTPMessageTemplate<SimpleContent>
    {
        public string GetSubject(SimpleContent content)
        {
            return content.Subject;
        }

        public string GetBody(SimpleContent content)
        {
            return content.Body;
        }

        public bool IsHtml { get; } = false;
        public bool CanHaveAttachments { get; } = true;
    }
}