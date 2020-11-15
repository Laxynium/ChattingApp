using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.AddRole
{
    public class AddRoleCommand : ICommand
    {
        public Guid RoleId { get; }
        public Guid MemberId { get; }
        public string Name { get; }

        public AddRoleCommand(Guid roleId, Guid memberId, string name)
        {
            RoleId = roleId;
            MemberId = memberId;
            Name = name;
        }
    }

    internal sealed class AddRoleHandler : ICommandHandler<AddRoleCommand>
    {
        public AddRoleHandler()
        {
            
        }
        public async Task HandleAsync(AddRoleCommand command)
        {
            var roleName = RoleName.Create(command.Name);
            //I need to verify if given member can perform this command
            //I want to place this role as the last one
        }
    }
}