using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Profiles.Domain
{
    public class Avatar : ValueObject
    {

        protected override IEnumerable<object> GetEqualityComponents()
        {
            return new object[]{ };
        }
    }
}