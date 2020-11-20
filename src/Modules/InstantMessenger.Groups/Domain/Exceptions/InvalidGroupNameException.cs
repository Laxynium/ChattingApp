namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidGroupNameException : DomainException
    {
        public string Name { get; }
        public override string Code => "invalid_group_name";

        public InvalidGroupNameException(string name) : base($"'{name}' is invalid group name.")
        {
            Name = name;
        }
    }
}