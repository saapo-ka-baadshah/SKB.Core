using System.Diagnostics;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.DependencyInjection;

namespace SKB.Core.Hosting.Extensions.Instrumentations;

/// <summary>
/// Adds the injectable Core Instrumentations to the service collections
/// </summary>
public static class InstrumentationExtensions
{

	/// <summary>
	/// Adds the instrumentation sources to the service collection to inject as Dependencies
	/// </summary>
	/// <param name="services">Service Collection <see cref="IServiceCollection"/></param>
	/// <returns>Extended Service Collection</returns>
	public static IServiceCollection AddInstrumentation<TAssembly>(this IServiceCollection services)
	{
		// Add Activity Source for Tracing
		services.AddScoped<ActivitySource>(sp => new ActivitySource(
			$"{typeof(TAssembly).Namespace}.Activity"
			));

		// Add Metering Source for Metrics
		services.AddScoped<Meter>(sp => new Meter(
			$"{typeof(TAssembly).Namespace}.Meter"
			));
		return services;
	}
}
