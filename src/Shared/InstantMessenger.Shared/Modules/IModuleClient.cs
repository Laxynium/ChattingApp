using System;
using System.Threading.Tasks;

namespace InstantMessenger.Shared.Modules
{
    public interface IModuleClient
    {
        Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class;
        Task PublishAsync(object moduleBroadcast);
    }


    public interface IModuleSubscriber
    {
        IModuleSubscriber Subscribe<TRequest>(string path, Func<IServiceProvider, TRequest, Task<object>> action) where TRequest : class;
    }
}