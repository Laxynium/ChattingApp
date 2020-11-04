using System;
using System.Threading.Tasks;
using InstantMessenger.Api;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Profiles;
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

            await ResetCheckpoint();
            await InitDb<IdentityContext>();
            await InitDb<ProfilesContext>();
        }

        public Task DisposeAsync()
        {
            _testServer.Dispose();
            return Task.CompletedTask;
        }

        private async Task InitDb<TDbContext>() where TDbContext : DbContext
        {
            using var scope = _testServer.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<TDbContext>();
            await context.Database.MigrateAsync();
        }

        private async Task ResetCheckpoint()
        {
            var checkpoint = new Checkpoint
            {
                DbAdapter = DbAdapter.SqlServer,
                TablesToIgnore = new[]
                {
                    "__EFMigrationsHistory",
                },
                WithReseed = true
            };
            using var scope = _testServer.Services.CreateScope();
            var connectionString = scope.ServiceProvider.GetService<IConfiguration>().GetConnectionString("InstantMessengerDb");
            await checkpoint.Reset(connectionString);
        }
    }
}