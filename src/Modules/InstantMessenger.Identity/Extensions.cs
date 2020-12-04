using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Identity.Api;
using InstantMessenger.Identity.Api.Features.SignIn;
using InstantMessenger.Identity.Api.Features.SignUp;
using InstantMessenger.Identity.Api.Queries;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Identity.Infrastructure;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MailKit;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.Queries;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace InstantMessenger.Identity
{
    public static class Extensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services)
        {
            var identityOptions = services.GetOptions<IdentityOptions>(nameof(IdentityOptions));
            var hubPaths = services.GetOptions<List<string>>("HubEndpoints");
            services.AddSingleton(identityOptions);
            services.AddControllers()
                .AddControllersAsServices()
                .AddNewtonsoftJson();

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
                                    .ForEach(x=>context.Token = accessToken);
                            }
                            
                            return Task.CompletedTask;
                        }
                    };
                }
            );
            services
                .AddCommandHandlers()
                .Decorate(typeof(ICommandHandler<>),typeof(CommandHandlerTransactionDecorator<>))
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddModuleRequests()
                .AddExceptionMapper<ExceptionMapper>()
                .AddMailKit()
                .AddHttpContextAccessor()
                .AddMemoryCache()
                .AddDbContext<IdentityContext>(
                    o =>
                    {
                        using var provider = services.BuildServiceProvider();
                        using var scope = provider.CreateScope();
                        var connectionString = scope.ServiceProvider.GetService<IConfiguration>().GetConnectionString("InstantMessengerDb");
                        o.UseSqlServer(connectionString, x=>x.MigrationsHistoryTable("__EFMigrationsHistory", "Identity"));
                    }
                )
                .AddScoped<IUniqueEmailRule, UniqueEmailRule>()
                .AddScoped<IUniqueNicknameRule, UniqueNicknameRule>()
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IActivationLinkRepository, ActivationLinkRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<RandomStringGenerator>()
                .AddSingleton<IPasswordHasher<User>>(s => new PasswordHasher<User>())
                .AddTransient<IAuthTokenService, AuthTokenService>()
                .AddTransient<IAuthTokensCache, AuthTokensCache>()
                .AddScoped<LinkGenerator>()
                .AddSingleton(identityOptions);

            return services;
        }

        public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app)
        {
            app.UseCors();
            app.UseAuthentication(); 
            app.UseAuthorization();
            app.UseModuleRequests()
                .Subscribe<MeQuery>("/identity/me", async (sp, q) => await sp.GetRequiredService<IQueryDispatcher>().QueryAsync(q))
                .Subscribe<GetUserQuery>("/identity/users", async (sp, q) => await sp.GetRequiredService<IQueryDispatcher>().QueryAsync(q));
            return app;
        }
    }
}