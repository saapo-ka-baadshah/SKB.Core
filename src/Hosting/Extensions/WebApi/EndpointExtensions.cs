using Microsoft.AspNetCore.Routing;
using SKB.Core.Abstractions.WebApi;

namespace SKB.Core.Hosting.Extensions.WebApi;

/// <summary>
/// Extends the usage of the WebApplication Router to allow discovering Endpoints
/// </summary>
public static class EndpointExtensions
{
	/// <summary>
	/// Adds all the Endpoints of the base type to the WebApi
	/// Definition of an Endpoint is to be cast with <see cref="IBaseEndpoint"/>
	/// </summary>
	/// <param name="app">WebApplication to which the endpoint is being added</param>
	/// <typeparam name="TAssembly">Assembly which contains the endpoints</typeparam>
	/// <returns></returns>
	public static IEndpointRouteBuilder AddEndpoints<TAssembly>(this IEndpointRouteBuilder app)
	{
		var endpoints = typeof(TAssembly).Assembly
			.GetTypes()
			.Where(t => typeof(IBaseEndpoint).IsAssignableFrom(t) && t.IsClass)
			.Select(Activator.CreateInstance)
			.Cast<IBaseEndpoint>();

		foreach (var endpoint in endpoints)
		{
			endpoint.RegisterAllMethods(app);
		}

		return app;
	}

	/// <summary>
	/// Adds a specific single Endpoint
	/// Definition of an Endpoint is to be cast with <see cref="IBaseEndpoint"/>
	/// </summary>
	/// <param name="app">WebApplication to which the endpoint is being added</param>
	/// <param name="endpoint">Endpoint to be added</param>
	/// <typeparam name="TAssembly">Assembly which contains the endpoints</typeparam>
	/// <returns></returns>
	public static IEndpointRouteBuilder AddEndpoint<TAssembly>(this IEndpointRouteBuilder app, IBaseEndpoint endpoint)
	{
		endpoint.RegisterAllMethods(app);
		return app;
	}

	/// <summary>
	/// Adds all Endpoints which are of a specific type
	/// Definition of an Endpoint is to be cast with <see cref="IBaseEndpoint"/>
	/// </summary>
	/// <param name="app">WebApplication to which the endpoint is being added</param>
	/// <typeparam name="TAssembly">Assembly which contains the endpoints</typeparam>
	/// <typeparam name="TIEndpoint"></typeparam>
	/// <returns></returns>
	public static IEndpointRouteBuilder AddEndpoints<TAssembly, TIEndpoint>
		(this IEndpointRouteBuilder app)
	where TIEndpoint : IBaseEndpoint
	{
		var endpoints = typeof(TAssembly).Assembly
			.GetTypes()
			.Where(t => typeof(TIEndpoint).IsAssignableFrom(t) && t.IsClass)
			.Select(Activator.CreateInstance)
			.Cast<IBaseEndpoint>();

		foreach (var endpoint in endpoints)
		{
			endpoint.RegisterAllMethods(app);
		}

		return app;
	}
}
