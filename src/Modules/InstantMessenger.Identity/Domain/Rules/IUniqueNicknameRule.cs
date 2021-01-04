using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.ValueObjects;

namespace InstantMessenger.Identity.Domain.Rules
{
    public interface IUniqueNicknameRule
    {
        Task<bool> IsMeet(Nickname nickname);
    }
}