using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Identity.Api.Queries
{
    public class UserDto2
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; } //base64 encoded image byte[]
    }
    public class GetUserByIdQuery: IQuery<UserDto2>
    {
        public Guid UserId { get; }

        public GetUserByIdQuery(Guid userId)
        {
            UserId = userId;
        }
    }

    public sealed class GetUserByIdHandler : IQueryHandler<GetUserByIdQuery, UserDto2>
    {
        private readonly IdentityContext _context;

        public GetUserByIdHandler(IdentityContext context)
        {
            _context = context;
        }
        public async Task<UserDto2> HandleAsync(GetUserByIdQuery query)
        {
            var user = await _context.Users.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == query.UserId);
            if (user is null)
                return null;

            return new UserDto2
            {
                Id = user.Id,
                Nickname = user.Nickname,
                Avatar = user.Avatar?.AsBase64String(),
            };
        }
    }
}