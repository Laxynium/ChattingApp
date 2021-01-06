using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Rules
{
    public interface IUniqueInvitationCodeRule
    {
        Task<bool> IsMeet(InvitationCode invitationCode);
    }
}