using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.UnitTests
{
    internal sealed class FakeModuleClient : IModuleClient
    {
        private readonly string _nickname;

        public FakeModuleClient(string nickname)
        {
            _nickname = nickname;
        }
        public Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class
        {
            var result = moduleRequest switch
            {
                MeQuery q => new UserDto(q.UserId, _nickname)
            };
            return Task.FromResult((TResult) System.Convert.ChangeType(result, typeof(TResult)));
        }

        public Task PublishAsync(object moduleBroadcast)
        {
            return Task.CompletedTask;
        }
    }
}