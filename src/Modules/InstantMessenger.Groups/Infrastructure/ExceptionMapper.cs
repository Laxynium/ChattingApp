using System;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Groups.Infrastructure
{
    public class ExceptionMapper: IExceptionMapper
    {
        public Error Map(Exception exception) => exception switch
        {
            DomainException e => new Error(e.Message, e.Code),
            _ => null
        };
    }
}