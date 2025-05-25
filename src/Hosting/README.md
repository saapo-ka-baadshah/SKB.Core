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

