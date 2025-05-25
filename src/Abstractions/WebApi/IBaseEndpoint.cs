using Microsoft.AspNetCore.Routing;

namespace SKB.Core.Abstractions.WebApi;

/// <summary>
/// The Base level Endpoint interface for the Assembly Scanning
/// </summary>
public interface IBaseEndpoint
{
	/// <summary>
	/// Registers all the Methods to the specific routes
	/// </summary>
	/// <param name="app">WebApplication to which the route is being added</param>
	void RegisterAllMethods(IEndpointRouteBuilder app);
}
