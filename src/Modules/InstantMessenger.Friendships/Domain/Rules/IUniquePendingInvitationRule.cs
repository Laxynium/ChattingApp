using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;

namespace InstantMessenger.Friendships.Domain.Rules
{
    public interface IUniquePendingInvitationRule
    {
        Task<bool> IsMet(PersonId sender, PersonId receiver);
    }
}