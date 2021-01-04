using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Api.Queries
{
    public class GetUserQuery : IQuery<UserDto>
    {
        public string Nickname { get; }

        public GetUserQuery(string nickname)
        {
            Nickname = nickname;
        }
    }

    public class GetUserHandler : IQueryHandler<GetUserQuery, UserDto>
    {
        private readonly IdentityContext _context;

        public GetUserHandler(IdentityContext context)
        {
            _context = context;
        }
        public async Task<UserDto> HandleAsync(GetUserQuery query) 
            => await _context.Users
                .AsNoTracking()
                .Where(x => x.Nickname == Nickname.Create(query.Nickname))
            .Select(x=>new UserDto
                {
                    Id = x.Id,
                    Email = x.Email,
                    Nickname = x.Nickname
            }
            )
            .FirstOrDefaultAsync();
    }
}