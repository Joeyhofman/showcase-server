# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ENV ContinuousIntegrationBuild=true
ENV BuildProjectReferences=true
ENV DisableGitVersionTask=true
ENV EnableSourceControlManagerQueries=false
WORKDIR /src

# Copy project files first for better layer caching
COPY API/API.csproj API/
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/

# Restore dependencies
RUN dotnet restore API/API.csproj

# Copy everything else
COPY . .

# Publish API
WORKDIR /src/API
RUN dotnet publish API.csproj \
    -c Release \
    -o /app/publish \
    /p:EnableSourceControlManagerQueries=false \
    /p:SourceLinkCreate=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "API.dll"]