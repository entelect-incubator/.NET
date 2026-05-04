# Phase 15: GitHub Container Registry Publishing

This phase teaches you how to **containerize and publish the Pezza API** to GitHub Container Registry (GHCR) using **GitHub Actions CI/CD**. You'll learn Docker multi-stage builds, automated image publishing, versioning strategies, and how frontend incubators consume the published API image.

Requires .NET SDK: 10.0.x (Docker handles runtime)

Estimated time: 2–4 hours — Difficulty: ★★★☆☆

Prerequisites: Complete Phase 1-14, understand Docker basics, and have a GitHub repository configured.

## Purpose

Phase 15 demonstrates how to containerize the Pezza API (from Phase 14 Final Solution) and publish it to GitHub Container Registry (GHCR). This enables all frontend incubators to consume a stable, published API image without rebuilding.

Frontend teams no longer need to build the API themselves—they simply pull the published image and integrate it into their docker-compose setup. This phase teaches the infrastructure and CI/CD patterns that enable this consumption model.

## Learning Objectives

By the end of this phase, you will understand:

- **Docker containerization** - Building production-ready API images
- **GitHub Actions CI/CD** - Automated image building and publishing
- **GitHub Container Registry** - Publishing and managing container images
- **Image versioning** - Semantic versioning and tagging strategies
- **Consumption patterns** - How frontends pull and use the published API image

## Architecture

```md
Phase 14 (Final Solution)
        ↓
    Dockerfile
        ↓
  Build Docker Image
        ↓
  GitHub Actions Workflow
        ↓
  GitHub Container Registry (ghcr.io)
        ↓
  Frontend Incubators Pull & Use
```

## Phase Structure

```md
Phase 15/
├── README.md (this file)
├── Dockerfile
├── .dockerignore
├── .github/
│   └── workflows/
│       └── ghcr-publish.yml
└── src/
    └── 01. StartSolution/
        └── (Copy of Phase 14 Final Solution)
```

## Key Files

### Dockerfile

Multi-stage build for optimal image size:

```dockerfile
# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /build
COPY . .
RUN dotnet restore
RUN dotnet build -c Release
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=builder /app .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
HEALTHCHECK --interval=30s --timeout=3s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1
CMD ["dotnet", "Pezza.Api.dll"]
```

### GitHub Actions Workflow (`ghcr-publish.yml`)

```yaml
name: Publish API to GitHub Container Registry

on:
  push:
    branches:
      - main
    paths:
      - '.NET/Phase 15/src/**'
      - '.NET/Phase 15/Dockerfile'
      - '.github/workflows/ghcr-publish.yml'
  workflow_dispatch:

env:
  REGISTRY: ghcr.io
  IMAGE_NAME: entelect-incubator/pezza-api

jobs:
  build-and-push:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Set up Docker Buildx
        uses: docker/setup-buildx-action@v3

      - name: Log in to GitHub Container Registry
        uses: docker/login-action@v3
        with:
          registry: ${{ env.REGISTRY }}
          username: ${{ github.actor }}
          password: ${{ secrets.GITHUB_TOKEN }}

      - name: Extract metadata
        id: meta
        uses: docker/metadata-action@v5
        with:
          images: ${{ env.REGISTRY }}/${{ env.IMAGE_NAME }}
          tags: |
            type=semver,pattern={{version}}
            type=semver,pattern={{major}}.{{minor}}
            type=sha,prefix={{branch}}-
            type=ref,event=branch
            type=raw,value=latest,enable={{is_default_branch}}

      - name: Build and push Docker image
        uses: docker/build-push-action@v5
        with:
          context: ./.NET/Phase 15
          push: true
          tags: ${{ steps.meta.outputs.tags }}
          labels: ${{ steps.meta.outputs.labels }}
          cache-from: type=gha
          cache-to: type=gha,mode=max
```

## Publishing Process

### 1. Automatic Publishing (Recommended)

The GitHub Actions workflow automatically:

- Triggers on `push` to `main` branch (or when `.github/workflows/ghcr-publish.yml` changes)
- Builds the Docker image from Dockerfile
- Tags with semantic version, git SHA, and `latest`
- Pushes to `ghcr.io/entelect-incubator/pezza-api`

### 2. Manual Publishing

```bash
# Build locally
docker build -t pezza-api:latest ./.NET/Phase\ 15

# Tag for registry
docker tag pezza-api:latest ghcr.io/entelect-incubator/pezza-api:latest

# Login to GitHub Container Registry
echo $GITHUB_TOKEN | docker login ghcr.io -u <username> --password-stdin

# Push to registry
docker push ghcr.io/entelect-incubator/pezza-api:latest
```

## Image Versioning Strategy

| Tag           | Usage                  | Example                                          |
| ------------- | ---------------------- | ------------------------------------------------ |
| `latest`      | Current stable release | ghcr.io/entelect-incubator/pezza-api:latest      |
| `v1.0.0`      | Semantic version       | ghcr.io/entelect-incubator/pezza-api:v1.0.0      |
| `main-abc123` | Branch + commit SHA    | ghcr.io/entelect-incubator/pezza-api:main-abc123 |
| `v1.0`        | Major.minor            | ghcr.io/entelect-incubator/pezza-api:v1.0        |

## Consuming the Published Image

### For Frontend Teams

```yaml
# docker-compose.yml
version: '3.8'
services:
  api:
    image: ghcr.io/entelect-incubator/pezza-api:latest
    ports:
      - "5000:5000"
    environment:
      - ConnectionStrings__DefaultConnection=Server=db;Port=5432;User Id=postgres;Password=postgres;Database=pezza;
    depends_on:
      - db

  db:
    image: ghcr.io/entelect-incubator/pezza-db:latest
    ports:
      - "5432:5432"
    environment:
      - POSTGRES_PASSWORD=postgres
```

### In GitHub Actions Workflows

```yaml
services:
  api:
    image: ghcr.io/entelect-incubator/pezza-api:latest
    options: >-
      --health-cmd="curl -f http://localhost:5000/health || exit 1"
      --health-interval=10s
```

## Configuration

### Environment Variables

The API image supports these environment variables:

```bash
# Database connection
ConnectionStrings__DefaultConnection=Server=db;Port=5432;User Id=postgres;Password=postgres;Database=pezza;

# ASP.NET Core settings
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
ASPNETCORE_HTTPS_PORT=5001

# Logging
LOG_LEVEL=Information

# OpenTelemetry
OTEL_EXPORTER_OTLP_ENDPOINT=http://otel-collector:4317
```

---

Teaching Thread

- From: Phase 14 integrated external services and resilient patterns.
- This phase: containerize services, set up GHCR publishing and deployable artifacts.
- Next: roll-up and maintain the incubator content; provide maintenance and contribution guidance.

Libraries (why they matter)

- Docker multi-stage builds: produce small, secure images for publishing.
- GitHub Actions: CI/CD pipelines for build, test, and publish; document secrets and GHCR usage.

Clean Code & SOLID (teaching notes)

- Keep deployment scripts declarative and idempotent; document expected environment variables and secrets.
- Ensure observability pieces are enabled in production images (OTel, logs).

MediatR policy

- CI/CD and container publishing are orthogonal to MediatR. If MediatR remains anywhere, add a migration item to remove or document it.

Notes

- Include example workflows for build+publish and local image testing.

### Secrets for GitHub Actions

To publish to the registry, configure these secrets:

1. **GITHUB_TOKEN** (automatic) - Provided by GitHub Actions
2. **DOCKER_REGISTRY_URL** (optional) - Override default registry

## Best Practices

### Security

- ✅ Use GitHub Token for GHCR authentication (automatic in Actions)
- ✅ Pin image versions in docker-compose (avoid `latest` in production)
- ✅ Scan images for vulnerabilities: `docker scan ghcr.io/entelect-incubator/pezza-api:latest`
- ✅ Use minimal base image (aspnet:8.0 vs sdk:8.0 for runtime)

### Performance

- ✅ Multi-stage builds reduce image size (~200MB vs 1GB+)
- ✅ Layer caching in GitHub Actions speeds up rebuilds
- ✅ Healthcheck ensures container readiness before routing traffic

### Maintenance

- ✅ Semantic versioning for production-critical images
- ✅ Update base image regularly: `mcr.microsoft.com/dotnet/aspnet:8.0`
- ✅ Document breaking changes in release notes

## Troubleshooting

### Image Won't Build

Check workflow logs:
```bash
# GitHub Actions logs → Workflow name → Push API to GitHub Container Registry
```

### Authentication Failed

```bash
# Verify GitHub Token permissions
# Settings → Developer Settings → Personal Access Tokens
# Required scopes: packages:read, packages:write
```

### Image Too Large

Optimize Dockerfile:

- Remove unnecessary files (`.dockerignore`)
- Delete build artifacts in runtime stage
- Use slim/alpine base images if compatible

### Version Tags Not Applied

Ensure git tags follow semantic versioning:

```bash
git tag v1.0.0
git push origin v1.0.0
```

## Next Steps

1. **Review Phase 14 Final Solution** - Understand the API being containerized
2. **Configure GitHub Actions** - Set up repository secrets and workflow
3. **Build & Test Locally** - Verify image runs correctly
4. **Publish First Image** - Push to ghcr.io
5. **Update Frontend Incubators** - Point to published API image
6. **Monitor Registry** - Track image size and versioning

## References

- [GitHub Container Registry Docs](https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry)
- [Docker Build Best Practices](https://docs.docker.com/build/building/best-practices/)
- [GitHub Actions Container Services](https://docs.github.com/en/actions/using-containerized-services)
- [ASP.NET Core Docker Images](https://mcr.microsoft.com/product/dotnet/aspnet/about)
- [Phase 14 - Final Solution API Documentation](../.NET/Phase%2014/)
