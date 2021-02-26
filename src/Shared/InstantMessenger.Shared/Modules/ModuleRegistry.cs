﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Shared.Modules
{
    internal sealed class ModuleRegistry : IModuleRegistry
    {
        private readonly ILogger<IModuleRegistry> _logger;
        private readonly IDictionary<string, ModuleRequestRegistration> _requestActions;
        private readonly IList<ModuleBroadcastRegistration> _broadcastActions;

        public ModuleRegistry(ILogger<ModuleRegistry> logger)
        {
            _logger = logger;
            _requestActions = new Dictionary<string, ModuleRequestRegistration>();
            _broadcastActions = new List<ModuleBroadcastRegistration>();
        }
        public ModuleRequestRegistration GetRequestRegistration(string path)
            => _requestActions[path];

        public IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string path) 
            => _broadcastActions.Where(r => r.Path == path);

        public bool TryAddRequestAction(string path, Type receiverType, Func<IServiceProvider, object, Task<object>> action)
        {
            var registration = new ModuleRequestRegistration
            {
                ReceiverType = receiverType,
                Action = action
            };

            var isValid = _requestActions.TryAdd(path, registration);
            if (isValid)
            {
                _logger.LogTrace($"Added module request of type {{{registration.ReceiverType.Name}}} at '{path}' path");
            }
            else
            {
                _logger.LogWarning($"Unabled to add module request of type " +
                                   $"{{{registration.ReceiverType.Name}}} at '{path}' due to path collision");
            }

            return isValid;
        }

        public void AddBroadcastAction(Type receiverType, Func<IServiceProvider, object, Task> action)
        {
            var registration = new ModuleBroadcastRegistration
            {
                ReceiverType = receiverType,
                Action = action
            };

            _broadcastActions.Add(registration);
            _logger.LogTrace($"Added broadcast of type {{{registration.ReceiverType.Name}}} to module registry");
        }
    }
}