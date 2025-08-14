using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using SKB.Core.Abstractions.Hosting.Auth;

namespace SKB.Core.Hosting.Extensions.Auth;

/// <summary>
/// Allows roles base resolution for the authorized resources
/// </summary>
public class RoleClaimsTransformation: IClaimsTransformation
{
	/// <inheritdoc />
	public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)		// purely state-task
	{
		var identity = principal.Identity as ClaimsIdentity;

		// JWT token after decoding will have the roles located under the name 'realm_access'
		var realmAccessClaim = identity?.FindFirst(RealmAccess.RealmAccessClaimKeyword);

		if (realmAccessClaim is not null)
		{
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true	// Ignore case when deserializing JSON
			};
			// Deserialize the realm_access JSON to extract the roles
			var realmAccess = JsonSerializer.Deserialize<RealmAccess>(realmAccessClaim.Value, options);

			if (realmAccess?.Roles is not null)
			{
				foreach (var role in realmAccess.Roles)
				{
					// Add each of the role to the claim for claiming identity
					identity!.AddClaim(new Claim(ClaimTypes.Role, role));
				}
			}
		}

		return Task.FromResult(principal);
	}
}
