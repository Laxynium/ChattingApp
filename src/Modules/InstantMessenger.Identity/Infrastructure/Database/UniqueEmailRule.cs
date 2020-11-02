using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Domain.ValueObjects;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    public class UniqueEmailRule : IUniqueEmailRule
    {
        public Task<bool> IsMet(Email email)
        {
            return Task.FromResult(true);
        }
    }
}