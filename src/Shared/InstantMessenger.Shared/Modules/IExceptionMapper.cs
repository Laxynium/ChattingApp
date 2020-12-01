using System;

namespace InstantMessenger.Shared.Modules
{
    public class Error
    {
        public string Message { get; }
        public string Code { get; }

        public Error(string message, string code)
        {
            Message = message;
            Code = code;
        }
    }
    public interface IExceptionMapper
    {
        Error Map(Exception exception);
    }
}