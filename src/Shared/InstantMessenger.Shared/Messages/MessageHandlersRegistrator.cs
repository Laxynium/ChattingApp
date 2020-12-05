using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using InstantMessenger.Shared.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages
{
    public class MessageHandlersRegistrator
    {
        private readonly Type _messageHandlerType;
        public MessageHandlersRegistrator(Type messageHandlerType)
        {
            _messageHandlerType = messageHandlerType;
        }
        public IServiceCollection Register(IServiceCollection services, Assembly callingAssembly, IEnumerable<Type> decorators)
        {
            var handlerTypes = callingAssembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(IsHandlerInterface))
                .Where(x => !x.IsDefined(typeof(DecoratorAttribute), true))
                .ToList();
            var decoratorsList = decorators.ToList();
            foreach (var handlerType in handlerTypes)
            {
                AddHandler(services, handlerType, decoratorsList);
            }

            return services;
        }

        private void AddHandler(IServiceCollection services, Type type, List<Type> decorators)
        {
            var pipeline = decorators
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            var interfaceType = type.GetInterfaces().Single(IsHandlerInterface);
            Func<IServiceProvider, object> factory = BuildPipeline(pipeline, interfaceType);

            services.AddTransient(interfaceType, factory);
        }

        private Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
        {
            var ctors = pipeline
                .Select(x =>
                {
                    Type type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
                    return type.GetConstructors().Single();
                })
                .ToList();

            Func<IServiceProvider, object> func = provider =>
            {
                object current = null;

                foreach (var ctor in ctors)
                {
                    var parameterInfos = ctor.GetParameters().ToList();

                    var parameters = GetParameters(parameterInfos, current, provider);

                    current = ctor.Invoke(parameters);
                }

                return current;
            };

            return func;
        }

        private object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider provider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, provider);
            }

            return result;
        }

        private object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider provider)
        {
            Type parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
                return current;

            object service = provider.GetService(parameterType);
            if (service != null)
                return service;

            throw new ArgumentException($"Type {parameterType} not found");
        }

        private bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
                return false;

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == _messageHandlerType;
        }
    }
}