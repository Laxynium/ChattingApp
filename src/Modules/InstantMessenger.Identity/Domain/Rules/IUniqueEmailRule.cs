using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.ValueObjects;

namespace InstantMessenger.Identity.Domain.Rules
{
    public interface IUniqueEmailRule
    {
        Task<bool> IsMet(Email email);
    }
}