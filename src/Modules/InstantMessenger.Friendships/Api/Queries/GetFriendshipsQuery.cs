using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Api.Queries
{
    public class FriendshipDto
    {
        public Guid FriendshipId { get; set; }
        public Guid Me { get; set; }
        public Guid Friend { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
    public class GetFriendshipsQuery : IQuery<IEnumerable<FriendshipDto>>
    {
        public Guid PersonId { get; }

        public GetFriendshipsQuery(Guid personId)
        {
            PersonId = personId;
        }
    }

    public class GetFriendshipsHandler : IQueryHandler<GetFriendshipsQuery, IEnumerable<FriendshipDto>>
    {
        private readonly FriendshipsContext _context;

        public GetFriendshipsHandler(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<FriendshipDto>> HandleAsync(GetFriendshipsQuery query) =>
            await _context.Friendships.AsNoTracking()
                .Where(x => x.FirstPerson == query.PersonId || x.SecondPerson == query.PersonId)
                .Select(x => new FriendshipDto
                {
                    FriendshipId = x.Id,
                    Me = x.FirstPerson == query.PersonId ? x.FirstPerson : x.SecondPerson,
                    Friend = x.FirstPerson == query.PersonId ? x.SecondPerson : x.FirstPerson,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
    }
}