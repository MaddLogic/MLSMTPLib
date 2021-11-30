namespace MLSMTPLib
{
    public class HTMLMessage: IMLSMTPMessageTemplate<HTMLContent>
    {
        private readonly bool _isHtml = true;
        private readonly bool _canHaveAttachments = true;

        public string GetSubject(HTMLContent content)
        {
            return content.Subject;
        }

        public string GetBody(HTMLContent content)
        {
            return content.Body;
        }

        public bool IsHtml => _isHtml;
        public bool CanHaveAttachments => _canHaveAttachments;
    }
}