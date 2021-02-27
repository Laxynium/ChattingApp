using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Modules
{
    public interface IModule
    {
        string Name { get; }

        IServiceCollection ConfigureServices(IServiceCollection services);

        IApplicationBuilder UseMiddleware(IApplicationBuilder app);
    }
}