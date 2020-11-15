using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;

namespace InstantMessenger.Identity.Domain.Rules
{
    public interface IUniqueNicknameRule
    {
        Task<bool> IsMeet(Nickname nickname);
    }
}