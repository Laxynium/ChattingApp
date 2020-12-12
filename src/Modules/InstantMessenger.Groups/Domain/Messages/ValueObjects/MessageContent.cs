using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.Messages.ValueObjects
{
    public class MessageContent : ValueObject
    {
        public string Value { get; }

        public MessageContent(string value)
        {
            Value = value.Trim();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}