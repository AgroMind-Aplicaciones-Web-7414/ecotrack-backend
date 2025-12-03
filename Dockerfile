# ===== BUILD STAGE =====
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos la solución y el código completo
COPY . .

# Restauramos dependencias
RUN dotnet restore "ecotrack-platform.sln"

# Publicamos el API en modo Release
RUN dotnet publish "EcotrackPlatform.API/EcotrackPlatform.API.csproj" -c Release -o /app/publish

# ===== RUNTIME STAGE =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Railway usa un puerto dinámico → ASPNETCORE_URLS lo expone automáticamente
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

# Copiamos los artefactos publicados
COPY --from=build /app/publish .

# Ejecutamos el backend
ENTRYPOINT ["dotnet", "EcotrackPlatform.API.dll"]
