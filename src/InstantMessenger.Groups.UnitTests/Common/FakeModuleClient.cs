using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Features.Group.Create.ExternalQueries;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Groups.UnitTests.Common
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
                    MeQuery q => new UserDto(q.UserId, _userIdToNickname.ContainsKey(q.UserId) ?  _userIdToNickname[q.UserId] : _defaultNickname, FakeAvatar),
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
        private static string FakeAvatar => 
            "data:image/png;base64,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";
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