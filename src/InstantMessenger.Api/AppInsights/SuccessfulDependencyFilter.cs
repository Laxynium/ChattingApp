using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace InstantMessenger.Api.AppInsights
{
    public class SuccessfulDependencyFilter : ITelemetryProcessor
    {
        private readonly ITelemetryProcessor _next;

        public SuccessfulDependencyFilter(ITelemetryProcessor next)
        {
            _next = next;
        }
        public void Process(ITelemetry item)
        {
            if (!OkToSend(item))
                return;
            _next.Process(item);
        }

        private bool OkToSend(ITelemetry item)
        {
            if (item is DependencyTelemetry dependencyTelemetry)
            {
                return dependencyTelemetry.Success != true;
            }
            else
            {
                return true;
            }
        }
    }
}