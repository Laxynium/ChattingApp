﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Commands
{
    public static class Extensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services) 
            => services.Scan( s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services) 
            => services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
    }
}