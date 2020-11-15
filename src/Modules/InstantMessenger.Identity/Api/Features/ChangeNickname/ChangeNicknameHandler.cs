using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.ChangeNickname
{
    internal sealed class ChangeNicknameHandler : ICommandHandler<ChangeNicknameCommand>
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUniqueNicknameRule _rule;

        public ChangeNicknameHandler(IUserRepository repository, IUnitOfWork unitOfWork, IUniqueNicknameRule rule)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _rule = rule;
        }
        public async Task HandleAsync(ChangeNicknameCommand command)
        {
            var user = await _repository.GetAsync(command.UserId) ?? throw new UserNotFoundException();

            await user.Change(Nickname.Create(command.Nickname),_rule);

            await _unitOfWork.Commit();
        }
    }
}