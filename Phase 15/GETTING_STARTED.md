# Phase 15: Getting Started Guide

## Quick Start (5 minutes)

### 1. Build the Docker Image Locally

```bash
cd .NET/Phase\ 15

# Build the image
docker build -t pezza-api:local .

# Verify the build
docker images | grep pezza-api
```

### 2. Run with Docker Compose

```bash
# Start all services (API + Database + OpenTelemetry)
docker-compose up -d

# Check services are running
docker ps | grep pezza

# View logs
docker-compose logs -f pezza-api
```

### 3. Test the API

```bash
# Health check
curl http://localhost:5000/health

# List pizzas (example endpoint)
curl http://localhost:5000/api/pizzas

# View Jaeger traces (browser)
# http://localhost:16686
```

### 4. Stop Services

```bash
docker-compose down
```

## Development Workflow

### For .NET Developers

1. **Make changes** to Phase 14 Final Solution code
2. **Update Phase 15** version numbers (optional)
3. **Build locally**: `docker build -t pezza-api:dev .`
4. **Test with compose**: `docker-compose up -d`
5. **Push to main branch** → GitHub Actions builds and publishes

### For Frontend Teams

1. **Pull the published image**:
   ```bash
   docker pull ghcr.io/entelect-incubator/pezza-api:latest
   ```

2. **Create your own docker-compose.yml**:
   ```yaml
   version: '3.8'
   services:
     api:
       image: ghcr.io/entelect-incubator/pezza-api:latest
       ports:
         - "5000:5000"
     db:
       image: ghcr.io/entelect-incubator/pezza-db:latest
       ports:
         - "5432:5432"
   ```

3. **Start your development environment**:
   ```bash
   docker-compose up -d
   ```

## GitHub Actions Setup

### Prerequisites

The workflow uses `secrets.GITHUB_TOKEN` which is automatically created by GitHub Actions. No manual setup needed!

### How It Works

1. You push code to `main` branch
2. Workflow triggers automatically
3. Builds Docker image from Dockerfile
4. Tags with version/commit/latest
5. Pushes to `ghcr.io/entelect-incubator/pezza-api`

### Troubleshooting Workflow

Check workflow logs:
1. Navigate to repository **Actions** tab
2. Find workflow: "Publish Pezza API to GitHub Container Registry"
3. Click latest run
4. Review logs for errors

## Image Versioning

### Automatic Tags Applied by Workflow

| Trigger          | Tags                           |
| ---------------- | ------------------------------ |
| Push to main     | `latest`, `main-abc123`        |
| Git tag v1.0.0   | `v1.0.0`, `v1.0`, `1.0.0`      |
| Any other branch | `branch-name`, `branch-abc123` |

### Manual Versioning

To tag a specific version:

```bash
# Create git tag
git tag v1.0.0

# Push tag to trigger workflow
git push origin v1.0.0

# Workflow will tag image as:
# ghcr.io/entelect-incubator/pezza-api:v1.0.0
# ghcr.io/entelect-incubator/pezza-api:latest (if on default branch)
```

## Common Tasks

### Update the API

1. Modify code in `.NET/Phase 15/src/02. FinalSolution/`
2. Push to main branch
3. GitHub Actions automatically builds and publishes
4. New image available as `ghcr.io/entelect-incubator/pezza-api:latest`

### Pin to Specific Version

```yaml
# In your docker-compose.yml
services:
  api:
    image: ghcr.io/entelect-incubator/pezza-api:v1.0.0  # Pin to specific version
```

### Inspect Published Image

```bash
# Pull latest
docker pull ghcr.io/entelect-incubator/pezza-api:latest

# Inspect layers and metadata
docker inspect ghcr.io/entelect-incubator/pezza-api:latest

# Check image size
docker images ghcr.io/entelect-incubator/pezza-api:latest
```

### Monitor Image Size

```bash
# Build and check size
docker build -t pezza-api .
docker images pezza-api
```

Typical sizes:
- SDK stage: ~2GB (builder, discarded)
- Runtime image: ~200-250MB (published)

### Update Base Image

Keep the .NET runtime image current:

```dockerfile
# In Dockerfile, update this line
FROM mcr.microsoft.com/dotnet/aspnet:8.0  # Update version as needed
```

Then rebuild and push.

## Testing the Published Image

### Pull from Registry

```bash
# Authenticate (if needed)
echo $GITHUB_TOKEN | docker login ghcr.io -u <username> --password-stdin

# Pull latest version
docker pull ghcr.io/entelect-incubator/pezza-api:latest

# Run standalone
docker run -d \
  --name pezza-api \
  -p 5000:5000 \
  -e ConnectionStrings__DefaultConnection="Server=localhost;Port=5432;..." \
  ghcr.io/entelect-incubator/pezza-api:latest
```

### Integration Testing

Use in GitHub Actions workflows:

```yaml
services:
  api:
    image: ghcr.io/entelect-incubator/pezza-api:latest
    options: >-
      --health-cmd="curl -f http://localhost:5000/health || exit 1"
      --health-interval=10s
      --health-timeout=5s
      --health-retries=3
```

## Next Steps

### For API Developers

- [ ] Review Phase 14 Final Solution codebase
- [ ] Build image locally and test
- [ ] Push changes to main branch
- [ ] Verify workflow ran successfully
- [ ] Check image published to GHCR

### For Frontend Teams

- [ ] Pull the published API image
- [ ] Create docker-compose.yml in your project
- [ ] Start development environment
- [ ] Connect your frontend to http://api:5000
- [ ] Remove local API dependencies

## Support & References

- **Phase 14 API Code**: [Phase 14 Final Solution](../.NET/Phase%2014/)
- **Docker Documentation**: https://docs.docker.com
- **GitHub Container Registry**: https://docs.github.com/en/packages/working-with-a-github-packages-registry/working-with-the-container-registry
- **GitHub Actions**: https://docs.github.com/en/actions

## Troubleshooting

### "docker: command not found"

Install Docker Desktop:
- Windows/Mac: https://www.docker.com/products/docker-desktop
- Linux: `sudo apt-get install docker.io`

### Image won't build

```bash
# Check Dockerfile is in correct location
ls -la .NET/Phase\ 15/Dockerfile

# Check Phase 14 project files exist
ls -la .NET/Phase\ 15/src/02.\ FinalSolution/

# Build with verbose output
docker build -t pezza-api --progress=plain .
```

### Container exits immediately

```bash
# Check logs
docker logs pezza-api

# Check health
docker ps --no-trunc | grep pezza-api
```

### Can't access API from browser

```bash
# Verify container is running
docker ps | grep pezza-api

# Check port mapping
docker inspect pezza-api | grep PortBindings

# Test with curl
curl http://localhost:5000/health
```
