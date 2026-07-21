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


    /// <summary>
    /// Writes telemetry items to debug output.
    /// </summary>
    public static class TelemetryDebugWriter
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

            if (TelemetryDebugWriter.IsAttached() && TelemetryDebugWriter.IsLogging())
            {
                string prefix = "Application Insights Telemetry: ";
                string serializedTelemetry = JsonSerializer.SerializeAsString(telemetry);
                TelemetryDebugWriter.WriteLine(prefix + serializedTelemetry);
            }
        }

        static void WriteLine(string message)
        {
            Debugger.Log(0, "category", message + Environment.NewLine);
        }

        static bool IsLogging()
        {
            if (IsTracingDisabled)
            {
                return false;
            }

            return Debugger.IsLogging();
        }

        static bool IsAttached()
        {
            return Debugger.IsAttached;
        }
    }
}
