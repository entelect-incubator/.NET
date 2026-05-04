global using Aspire.Hosting;

namespace AspireHost;

/// <summary>
/// Extensions for configuring OpenTelemetry and other observability features.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Adds OpenTelemetry configuration to the project.
    /// Configures logging, metrics, and tracing.
    /// </summary>
    public static IResourceBuilder<ProjectResource> WithOpenTelemetry(this IResourceBuilder<ProjectResource> project)
    {
        // OpenTelemetry environment variables for logging
        project.WithEnvironment("OTEL_SDK_DISABLED", "false");
        project.WithEnvironment("OTEL_EXPORTER_OTLP_PROTOCOL", "http/protobuf");
        project.WithEnvironment("OTEL_EXPORTER_OTLP_ENDPOINT", "http://localhost:4318");

        return project;
    }
}
