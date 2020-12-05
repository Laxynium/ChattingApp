using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.UnitTests.Common
{
    internal sealed class FakeModuleClient : IModuleClient
    {
        private readonly string _defaultNickname;
        private readonly Dictionary<Guid,string> _userIdToNickname;

        public FakeModuleClient(string defaultNickname = "user_nickname", Dictionary<Guid, string> userIdToNickname = null)
        {
            _defaultNickname = defaultNickname;
            _userIdToNickname = userIdToNickname ?? new Dictionary<Guid, string>();
        }
        public Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class
        {
            var result = path switch
            {
                "/identity/me" => moduleRequest switch
                {
                    MeQuery q => new UserDto(q.UserId, _userIdToNickname.ContainsKey(q.UserId) ?  _userIdToNickname[q.UserId] : _defaultNickname),
                    _ => throw new ArgumentException($"There is no handler under given request[{moduleRequest.GetType().Name}]")
                },
                _ => throw new ArgumentException($"There is no resource under given path[{path}]")
            };

            return Task.FromResult((TResult) System.Convert.ChangeType(result, typeof(TResult)));
        }

        public Task PublishAsync(object moduleBroadcast)
        {
            return Task.CompletedTask;
        }
    }

    internal sealed class FakeMessageBroker : IMessageBroker
    {
        public Task PublishAsync(IIntegrationEvent @event)
        {
            return Task.CompletedTask;
        }

        public Task PublishAsync(IEnumerable<IIntegrationEvent> events)
        {
            return Task.CompletedTask;
        }
    }
}