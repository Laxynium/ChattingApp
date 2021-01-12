using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Groups.UnitTests.Common
{
    public abstract class UnitTestBase<TModuleFacade>
    {
        private readonly ServiceCollection _services = new ServiceCollection();

        protected void Configure(Action<ServiceCollection> config)
        {
            config(_services);
        }

        public async Task Run(Func<TModuleFacade, Task> func)
        {
            using var provider = _services.BuildServiceProvider();
            using var scope = provider.CreateScope();
            var sp = scope.ServiceProvider;
            var sut = sp.GetRequiredService<TModuleFacade>();
            await func(sut);
        }
    }
}