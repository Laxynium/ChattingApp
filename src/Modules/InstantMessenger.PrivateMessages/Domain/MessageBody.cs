using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace InstantMessenger.PrivateMessages.Domain
{
    public class MessageBody : ValueObject
    {
        public string TextContent { get; }

        public MessageBody(string textContent)
        {
            TextContent = textContent;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return TextContent;
        }
    }
}