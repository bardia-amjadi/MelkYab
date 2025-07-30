# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MelkYab.Backend/MelkYab.Backend.csproj", "MelkYab.Backend/"]
RUN dotnet restore "MelkYab.Backend/MelkYab.Backend.csproj"
COPY . .
WORKDIR "/src/MelkYab.Backend"
RUN dotnet build "MelkYab.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MelkYab.Backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MelkYab.Backend.dll"]
