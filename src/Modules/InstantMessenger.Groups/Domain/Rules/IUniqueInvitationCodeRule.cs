using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;

namespace InstantMessenger.Groups.Domain.Rules
{
    public interface IUniqueInvitationCodeRule
    {
        Task<bool> IsMeet(InvitationCode invitationCode);
    }
}