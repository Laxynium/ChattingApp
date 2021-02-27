using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Application.Features.SignUp;
using InstantMessenger.Identity.Application.IntegrationEvents;
using InstantMessenger.Identity.Application.Queries;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Infrastructure;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Identity.Infrastructure.Decorators;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.UoW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InstantMessenger.Identity
{
    internal sealed class IdentityModule : IModule
    {
        public string Name => "identity";

        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            var identityOptions = services.GetOptions<IdentityOptions>(nameof(IdentityOptions));
            var hubPaths = services.GetOptions<List<string>>("HubEndpoints");
            services.AddSingleton(identityOptions);

            services.AddCors(
                c =>
                {
                    c.AddDefaultPolicy(
                        x => x
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins(identityOptions.ClientAppUrlBase)
                    );
                }
            );

            services.AddAuthentication(
                o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(
                o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(identityOptions.Key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    o.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrWhiteSpace(accessToken))
                            {
                                hubPaths.Where(hubPath => path.StartsWithSegments(hubPath))
                                    .ToList()
                                    .ForEach(x => context.Token = accessToken);
                            }

                            return Task.CompletedTask;
                        }
                    };
                }
            );
            services
                .AddCommandHandlers(typeof(TransactionCommandHandlerDecorator<>))
                .AddCommandDispatcher()
                .AddDomainEventHandlers(typeof(PublishDomainEventsEventHandlerDecorator<>))
                .AddDomainEventDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddIntegrationEventHandlers()
                .AddIntegrationEventDispatcher()
                .AddModuleRequests()
                .AddExceptionMapper<ExceptionMapper>()
                .AddMailKit()
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddDbContext<IdentityContext>(
                    (provider, o) =>
                    {
                        o.UseSqlServer(provider.GetConnectionString("InstantMessengerDb"),
                            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
                    }
                )
                .AddUnitOfWork<IdentityModule, DomainEventMapper>(c => c.UseEFCore<IdentityContext>())
                .AddScoped<IUniqueEmailRule, UniqueEmailRule>()
                .AddScoped<IUniqueNicknameRule, UniqueNicknameRule>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IActivationLinkRepository, ActivationLinkRepository>()
                .AddSingleton<RandomStringGenerator>()
                .AddSingleton<IPasswordHasher<User>>(s => new PasswordHasher<User>())
                .AddTransient<IAuthTokenService, AuthTokenService>()
                .AddTransient<IAuthTokensCache, AuthTokensCache>()
                .AddScoped<LinkGenerator>()
                .AddSingleton(identityOptions);

            return services;
        }

        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseModuleRequests()
                .Subscribe<MeQuery>("/identity/me",
                    async (sp, q) => await sp.GetRequiredService<IQueryDispatcher>().QueryAsync(q))
                .Subscribe<GetUserQuery>("/identity/users",
                    async (sp, q) => await sp.GetRequiredService<IQueryDispatcher>().QueryAsync(q));
            return app;
        }
    }
}