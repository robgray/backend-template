using System;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Channel;
using Serilog.Events;
using Serilog.Sinks.ApplicationInsights.Sinks.ApplicationInsights.TelemetryConverters;

namespace core.Plumbing.Logging
{
    public class OperationTelemetryConverter : TraceTelemetryConverter
    {
        public override IEnumerable<ITelemetry> Convert(LogEvent logEvent, IFormatProvider formatProvider)
        {
            foreach (var telemetry in base.Convert(logEvent, formatProvider))
            {
                if (TryGetScalarProperty(logEvent, OperationIdEnricher.OperationId, out var operationId))
                {
                    var formattedOperationId = operationId?.ToString()?.Replace("\"", String.Empty);
                    telemetry.Context.Operation.Id = formattedOperationId;
                    telemetry.Context.Operation.ParentId = formattedOperationId;
                }

                yield return telemetry;
            }
        }

        private bool TryGetScalarProperty(LogEvent logEvent, string propertyName, out object? value)
        {
            var hasScalarValue = logEvent.Properties.TryGetValue(propertyName, out var someValue) &&
                                 someValue is ScalarValue;

            value = (hasScalarValue ?  someValue as ScalarValue : default);

            return hasScalarValue;
        }
    }
}