using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Domain.Rules
{
    public interface IUniquePendingInvitationRule
    {
        Task<bool> IsMet(Person sender, Person receiver);
    }
}