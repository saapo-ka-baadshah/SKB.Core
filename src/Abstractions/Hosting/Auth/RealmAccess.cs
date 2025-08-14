namespace SKB.Core.Abstractions.Hosting.Auth;

/// <summary>
/// Provides an abstraction for the Realm access from keycloak
/// </summary>
public class RealmAccess
{
	/// <summary>
	/// JWT token after decoding will have the role resolutions under the key 'realm_access'
	/// </summary>
	public static string RealmAccessClaimKeyword = "realm_access";

	/// <summary>
	/// Roles claimed on a particular keycloak realm
	/// </summary>
	public List<string>? Roles { get; set; }
}
