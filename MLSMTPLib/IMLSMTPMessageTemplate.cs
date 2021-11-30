using System;

namespace MLSMTPLib
{
    public interface IMLSMTPMessageTemplate<T> where T:IMessageContent
    {
        public string GetSubject(T content);
        public string GetBody(T content);
        bool IsHtml { get;}
        bool CanHaveAttachments { get;}
    }
}