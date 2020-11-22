﻿using InstantMessenger.PrivateMessages.Api;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.PrivateMessages
{
    public static class Extensions
    {
        public static IServiceCollection AddPrivateMessagesModule(this IServiceCollection services)
        {
            services.AddCommandHandlers()
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddEventHandlers()
                .AddModuleRequests()
                .AddDbContext<PrivateMessagesContext>(o =>
                {
                    using var provider = services.BuildServiceProvider();
                    using var scope = provider.CreateScope();
                    var connectionString = scope.ServiceProvider.GetService<IConfiguration>()
                        .GetConnectionString("InstantMessengerDb");
                    o.UseSqlServer(connectionString,
                        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "PrivateMessages"));
                })
                .AddScoped<IConversationRepository, ConversationRepository>()
                .AddScoped<IMessageRepository,MessageRepository>()
                .AddScoped<IUnitOfWork,UnitOfWork>();
            return services;
        }
    }
}