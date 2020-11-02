using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InstantMessenger.Shared.MailKit
{
    public static class Extensions
    {
        public static IServiceCollection AddMailKit(this IServiceCollection services)
        {
            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var configuration = scope.ServiceProvider.GetService<IConfiguration>();
                services.AddSingleton<IMailService, MailService>();
                services.Configure<MailKitOptions>(options => configuration.GetSection("MailSender").Bind(options));
                services.AddSingleton(
                    sp => new MailOptions(sp.GetService<IOptions<MailKitOptions>>().Value.Email)
                );
            }

            return services;
        }
    }
}