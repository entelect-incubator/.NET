# Multi-stage build for Pezza API

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /build

# Copy solution and project files
COPY src/02.FinalSolution/ .

# Restore dependencies
RUN dotnet restore

# Build
RUN dotnet build -c Release

# Publish
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy published files from builder
COPY --from=builder /app .

# Expose port
EXPOSE 5000

# Set environment
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1

# Run application
CMD ["dotnet", "Pezza.Api.dll"]
