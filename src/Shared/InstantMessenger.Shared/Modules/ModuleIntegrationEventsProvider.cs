using System;
using System.Collections.Generic;
using System.Linq;

namespace InstantMessenger.Shared.Modules
{
    internal class ModuleIntegrationEventsProvider
    {
        internal IEnumerable<Type> IntegrationEventTypes { get; }

        internal ModuleIntegrationEventsProvider(IEnumerable<Type>types)
        {
            IntegrationEventTypes = types.ToList();
        }
    }
}