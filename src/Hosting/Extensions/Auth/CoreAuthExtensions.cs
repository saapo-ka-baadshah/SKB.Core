using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace SKB.Core.Hosting.Extensions.Auth;

/// <summary>
/// Adds core authentication and authorization extensions
/// </summary>
public static class CoreAuthExtensions
{
	/// <summary>
	/// Adds core auth layer to the application
	/// </summary>
	/// <param name="services">Builder service collection.</param>
	/// <param name="configuration">Provides a builder configuration.</param>
	/// <returns>Services loaded.</returns>
	public static IServiceCollection AddCoreAuthLayer(this IServiceCollection services, IConfiguration configuration)
	{
		KeycloakOptions? options = configuration.GetSection("Keycloak").Get<KeycloakOptions>();

		if (options is not null)
		{
			// Add authorization
			services.AddAuthorization();

			// Add authentication
			services
				.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(o =>
				{
					o.RequireHttpsMetadata = false;
					o.Audience = options.Audience!;
					o.MetadataAddress = options.MetadataAddress!;
					o.TokenValidationParameters = new TokenValidationParameters
					{
						ValidIssuer = options.ValidIssuer!,
						ValidateIssuerSigningKey = true,
						ValidateLifetime = true,
						ValidateAudience = false,
						ValidateIssuer = true
					};
				});

			// Add the resolutions for the role based access
			services.AddTransient<IClaimsTransformation, RoleClaimsTransformation>();
		}

		return services;
	}
}
