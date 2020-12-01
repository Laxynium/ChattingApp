using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Identity.Api.Queries
{
    public sealed class MeQueryHandler : IQueryHandler<MeQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public MeQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> HandleAsync(MeQuery query)
        {
            var user = await _userRepository.GetAsync(query.UserId);
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Nickname = user.Nickname,
                Avatar = user.Avatar?.AsBase64String()
            };
        }
    }
}