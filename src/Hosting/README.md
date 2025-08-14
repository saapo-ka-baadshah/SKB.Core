# SKB.Core.Hosting

Provides a general set of extensions for Hosting

## Current Set
### 1. Configurations: ``ConfigrationExtensions``
Here we can add Specific configuration provider based on the standard ``appsettings`` conventions.

*i.e.*
Add a file with ``appsettings.json`` or ``appsettings.Development.json`` or ``appsettings.{Environment}.json``

### 2. OpenTelemetry: ``OpenTelemetryExtensions``
This provides standard OpenTelemetryOptions to the Application.

*i.e.*
Add a section in your ``appsettings.json`` in the root level:
````json
{
	...,
	"OTEL_EXPORTER_OTLP_ENDPOINT": "http://localhost:4317",			// Example OTEL Collector Endpoint
	...
}
````

### 3. WebApi: ``EndpointExtensions``
This provides an automatic inclusions extension for the Endpoints. Adding all Endpoints will Look for
all implementations of ``IBaseEndpoint``.

### 4. Auth: ``CoreAuthExtensions``
This provides core authentication extensions accessible by adding the configuration for the application.
This uses **Keycloak** as the Auth provider.
```json
{
	"Keycloak": {
		"MetadataAddress": "http://<address>:<port>/realms/<realm>/.well-known/openid-configuration",
		"ValidIssuer": "http://<address>:<port>/realms/<realm>",
		"Audience": "account",
		"AuthorizationUrl": "http://<address>:<port>/realms/<realm>/protocol/openid-connect/auth"
	}
}
```
