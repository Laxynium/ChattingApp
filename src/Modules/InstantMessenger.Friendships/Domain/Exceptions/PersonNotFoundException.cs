namespace InstantMessenger.Friendships.Domain.Exceptions
{
    public sealed class PersonNotFoundException : DomainException
    {
        public override string Code => "person_not_found";

        public PersonNotFoundException() : base("Person was not found.")
        {
        }
    }
}