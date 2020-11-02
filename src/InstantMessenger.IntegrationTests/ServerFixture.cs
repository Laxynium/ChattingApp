using System;
using System.Threading.Tasks;
using InstantMessenger.Api;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.MailKit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using RestEase;
using Xunit;

namespace InstantMessenger.IntegrationTests
{
    public class ServerFixture : IAsyncLifetime
    {
        private TestServer _testServer;

        public ServerFixture()
        {
        }
        public TApi GetClient<TApi>() 
            => RestClient.For<TApi>(_testServer.CreateClient());

        public TService GetService<TService>() => _testServer.Services.GetRequiredService<TService>();
        public IServiceProvider Services => _testServer.Services;

        public async Task InitializeAsync()
        {
            var webHostBuilder = new WebHostBuilder()
                .ConfigureAppConfiguration((c, b) => b.AddJsonFile("appsettings.json"))
                .ConfigureTestServices(services => { services.AddSingleton<IMailService, FakeMailService>(); })
                .UseStartup<Startup>();
            _testServer = new TestServer(webHostBuilder);

            await InitDb();
        }

        public Task DisposeAsync()
        {
            _testServer.Dispose();
            return Task.CompletedTask;
        }

        private async Task InitDb()
        {
            using var scope = _testServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<IdentityContext>();
            var checkpoint = new Checkpoint { DbAdapter = DbAdapter.SqlServer,TablesToIgnore = new []{ "__EFMigrationsHistory" }, WithReseed = true};
            await context.Database.OpenConnectionAsync();
            await context.Database.MigrateAsync();
            await checkpoint.Reset(context.Database.GetDbConnection());
        }
    }
}