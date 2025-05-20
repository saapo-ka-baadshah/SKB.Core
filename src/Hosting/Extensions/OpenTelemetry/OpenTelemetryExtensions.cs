using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace SKB.Core.Hosting.Extensions.OpenTelemetry;

/// <summary>
/// Extensions of the Hosting for OpenTelemetry
/// </summary>
public static class OpenTelemetryExtensions
{
	///  <summary>
	///  Add and Configure Open Telemetry Extensions
	///  </summary>
	///  <param name="builder">
	/// 		Provided Application Builder
	///  </param>
	public static void AddOpenTelemetry(
		this IHostApplicationBuilder builder
		)
	{
		string otlpEndpoint = builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"] ?? "";
		bool useOtlpEndpoint = !string.IsNullOrEmpty(otlpEndpoint);
		Action<OtlpExporterOptions>? otlpExporterOptionsAction = null;
		if (useOtlpEndpoint)
		{
			if (Uri.TryCreate(otlpEndpoint, UriKind.Absolute, out var otlpUri))
			{
				otlpExporterOptionsAction = (options) =>
				{
					options.Endpoint = otlpUri;
					// defaults to gRPC
					// defaults to No Auth
				};
			}
			else
			{
				Console.WriteLine($"[WARNING] Invalid OtlpEndpoint specified: {otlpEndpoint}");
				useOtlpEndpoint = false;
			}
		}
		if (!useOtlpEndpoint)
		{
			Console.WriteLine($"[WARNING] Skipping OtlpExport because OTLP endpoint is not configured. Falling back to default ConsoleExporter.");
		}

		// Configure Trace
		builder.Services.ConfigureOpenTelemetryTracerProvider(providerBuilder =>
		{
			if (useOtlpEndpoint && otlpExporterOptionsAction != null)
			{
				providerBuilder.AddOtlpExporter(otlpExporterOptionsAction);
			}
			else
			{
				providerBuilder.AddConsoleExporter();
			}
		});

		// Configure Metrics
		builder.Services.ConfigureOpenTelemetryMeterProvider(providerBuilder =>
		{
			if (useOtlpEndpoint && otlpExporterOptionsAction != null)
			{
				providerBuilder.AddOtlpExporter(otlpExporterOptionsAction);
			}
			else
			{
				providerBuilder.AddConsoleExporter();
			}
		});

		// Configure Logging
		builder.Logging.AddOpenTelemetry(loggingOptions =>
		{
			loggingOptions.IncludeFormattedMessage = true;
			loggingOptions.IncludeScopes = true;
			loggingOptions.ParseStateValues = true;

			if (useOtlpEndpoint && otlpExporterOptionsAction != null)
			{
				loggingOptions.AddOtlpExporter(otlpExporterOptionsAction);
			}
			else
			{
				loggingOptions.AddConsoleExporter();
			}
		});
	}
}
