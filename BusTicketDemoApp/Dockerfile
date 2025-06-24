# ==============================
# Dockerfile para BusTicketDemoApp API
# Optimizado para .NET Core 8 en Render
# ==============================

# Stage 1: Build - Imagen con SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivo de proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar código fuente y compilar
COPY . .
RUN dotnet build -c Release -o /app/build

# Stage 2: Publish - Crear artefactos de publicación
FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime - Imagen final ligera solo con runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copiar aplicación publicada
COPY --from=publish /app/publish .

# Variables de entorno para producción
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:10000
ENV DOTNET_RUNNING_IN_CONTAINER=true

# Exponer puerto (Render usa el puerto 10000 por defecto)
EXPOSE 10000

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "BusTicketDemoApp.dll"]
