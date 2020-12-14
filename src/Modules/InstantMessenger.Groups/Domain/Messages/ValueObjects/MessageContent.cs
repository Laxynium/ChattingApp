using System.Collections.Generic;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.Messages.ValueObjects
{
    public class MessageContent : ValueObject
    {
        public string Value { get; }

        private MessageContent(string value)
        {
            Value = value;
        }

        public static MessageContent From(string value) 
            => new MessageContent(value);

        public static MessageContent Create(string value)
        {
            value = value.Trim();
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidMessageException(value);
            }
            return new MessageContent(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}