using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public abstract string Name { get; }
        protected Member AsMember;

        protected Action(Member asMember)
        {
            AsMember = asMember;
        }

        public abstract bool CanExecute();

        public void Execute(System.Action action = null)
        {
            if (CanExecute())
            {
                action?.Invoke();
            }
            else
            {
                throw new InsufficientPermissionsException(AsMember.UserId);
            }
        }
    }
}