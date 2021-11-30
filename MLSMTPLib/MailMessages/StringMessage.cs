using System.Net.Mail;

namespace MLSMTPLib
{
    public class StringMessage: IMLSMTPMessageTemplate<SimpleContent>
    {
        public string GetSubject(SimpleContent content)
        {
            throw new System.NotImplementedException();
        }

        public string GetBody(SimpleContent content)
        {
            throw new System.NotImplementedException();
        }

        public bool IsHtml { get; } = false;
        public bool CanHaveAttachments { get; } = true;
    }
}