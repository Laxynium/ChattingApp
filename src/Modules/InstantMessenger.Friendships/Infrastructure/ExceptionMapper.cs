using System;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Friendships.Infrastructure
{
    public class ExceptionMapper: IExceptionMapper
    {
        public Error? Map(Exception exception) => exception switch
        {
            DomainException e => new Error(e.Message, e.Code),
            _ => null
        };
    }
}