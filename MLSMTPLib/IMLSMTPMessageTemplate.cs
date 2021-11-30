using System;

namespace MLSMTPLib
{
    public interface IMLSMTPMessageTemplate<T> where T:IMessageContent
    {
        string GetSubject(T content);
        string GetBody(T content);
        bool IsHtml { get;}
        bool CanHaveAttachments { get;}
    }
}