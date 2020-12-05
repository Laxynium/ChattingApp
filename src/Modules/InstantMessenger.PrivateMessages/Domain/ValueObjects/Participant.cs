using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace InstantMessenger.PrivateMessages.Domain.ValueObjects
{
    public class Participant : ValueObject
    {
        public Guid Id { get; }

        private Participant(){}
        public Participant(Guid id)
        {
            Id = id;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Id;
        }

        public static implicit operator Guid(Participant participant) =>
            participant.Id;
    }
}