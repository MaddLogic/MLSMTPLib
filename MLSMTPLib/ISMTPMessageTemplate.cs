namespace MaddLogic.MLSMTPLib
{
    public interface ISMTPMessageTemplate<T> where T:IMessageContent
    {
        string GetSubject(T content);
        string GetBody(T content);
        bool IsHtml { get;}
        bool CanHaveAttachments { get;}
    }
}