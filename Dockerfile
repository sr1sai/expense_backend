FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy all project files
COPY ["API/API.csproj", "API/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Repositories/Repositories.csproj", "Repositories/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["DatabaseContext/DatabaseContext.csproj", "DatabaseContext/"]

# Restore dependencies
RUN dotnet restore "API/API.csproj"

# Copy everything else (includes DatabaseSettings.prod.json)
COPY . .

# Build
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

# Copy published output (includes DatabaseSettings.prod.json via DatabaseContext.csproj)
COPY --from=publish /app/publish .

# Create non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "API.dll"]ENTRYPOINT ["dotnet", "API.dll"]