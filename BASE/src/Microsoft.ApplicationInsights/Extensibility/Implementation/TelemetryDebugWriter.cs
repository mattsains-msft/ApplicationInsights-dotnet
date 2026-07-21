// <copyright file="TelemetryDebugWriter.cs" company="Microsoft">
// Copyright © Microsoft. All Rights Reserved.
// </copyright>

#define DEBUG

namespace Microsoft.ApplicationInsights.Extensibility.Implementation
{
    using System;
    using System.Diagnostics;

    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.Extensibility.Implementation.Platform;

    /// <summary>
    /// Writes telemetry items to debug output.
    /// </summary>
    public class TelemetryDebugWriter : IDebugOutput
    {
        /// <summary>
        /// Gets or sets a value indicating whether writing telemetry items to debug output is enabled.
        /// </summary>
        public static bool IsTracingDisabled { get; set; }

        /// <summary>
        /// Write the specified <see cref="ITelemetry"/> item to debug output.
        /// </summary>
        /// <param name="telemetry">Item to write.</param>
        public static void WriteTelemetry(ITelemetry telemetry)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            var output = PlatformSingleton.Current.GetDebugOutput();
            if (output.IsAttached() && output.IsLogging())
            {
                string prefix = "Application Insights Telemetry: ";
                if (string.IsNullOrEmpty(telemetry.Context.InstrumentationKey))
                {
                    prefix = "Application Insights Telemetry (unconfigured): ";
                }

                string serializedTelemetry = JsonSerializer.SerializeAsString(telemetry);
                output.WriteLine(prefix + serializedTelemetry);
            }
        }

        void IDebugOutput.WriteLine(string message)
        {
            Debugger.Log(0, "category", message + Environment.NewLine);
        }

        bool IDebugOutput.IsLogging()
        {
            if (IsTracingDisabled)
            {
                return false;
            }

            return Debugger.IsLogging();
        }

        bool IDebugOutput.IsAttached()
        {
            return Debugger.IsAttached;
        }
    }
}
