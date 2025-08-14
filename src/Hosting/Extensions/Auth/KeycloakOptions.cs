namespace SKB.Core.Hosting.Extensions.Auth;

/// <summary>
/// Provides keycloak options
/// </summary>
public class KeycloakOptions
{
	/// <summary>
	/// Provides metadata address to retrieve OIDC information from keycloak
	/// </summary>
	public string? MetadataAddress { get; init; }

	/// <summary>
	/// Provides the issuer information from Keycloak
	/// </summary>
	public string? ValidIssuer { get; init; }

	/// <summary>
	/// Provides audience scope from Keycloak
	/// </summary>
	public string? Audience { get; init; }

	/// <summary>
	/// Provides an authorization url from the Keycloak
	/// </summary>
	public string? AuthorizationUrl { get; init; }
}
