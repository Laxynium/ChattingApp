using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class GroupAlreadyExistsException : DomainException
    {
        public GroupId GroupId { get; }
        public override string Code => "group_already_exists";

        public GroupAlreadyExistsException(GroupId groupId) : base($"Group[id={groupId}] already exists.")
        {
            GroupId = groupId;
        }
    }
}