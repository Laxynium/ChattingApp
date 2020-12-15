using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Role : Entity<RoleId>
    {
        public RoleName Name { get; private set; }
        public RolePriority Priority { get; private set; }
        public Permissions Permissions { get; private set; }

        public Role(RoleId id, RoleName name, RolePriority priority, Permissions permissions):base(id)
        {
            Name = name;
            Priority = priority;
            Permissions = permissions;
        }
        public static Role CreateEveryone()=>new Role(RoleId.Create(), RoleName.EveryOneRole, RolePriority.Lowest, Permissions.Empty());

        public static Role Create(RoleId id, RoleName name, RolePriority priority)
        {
            return new Role(id, name, priority, Permissions.Empty());
        }

        public void AddPermission(Permission permission)
        {
            Permissions = Permissions.Add(permission);
        }

        public void RemovePermission(Permission permission)
        {
            Permissions = Permissions.Remove(permission);
        }

        public void IncrementPriority()
        {
            if (this.Name == RoleName.EveryOneRole)
                return;
            Priority = Priority.Increased();
        }

        public void DecrementPriority()
        {
            if (this.Name == RoleName.EveryOneRole)
                return;
            Priority = Priority.Decreased();
        }

        public void Rename(RoleName roleName)
        {
            Name = roleName;
        }
    }
}