# ==============================
# Dockerfile Multi-Stage para BusTicketDemoApp
# SQL Server Express + ASP.NET Core API
# ==============================

# Stage 1: Build de la aplicación .NET
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto y restaurar dependencias
COPY *.csproj .
RUN dotnet restore

# Copiar código fuente y compilar
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime con SQL Server Express
FROM mcr.microsoft.com/mssql/server:2022-latest

# Variables de entorno para SQL Server
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=BusTicket2025!
ENV MSSQL_PID=Express
ENV MSSQL_TCP_PORT=1433

# Instalar .NET 8.0 Runtime y herramientas
RUN apt-get update && \
    apt-get install -y wget curl && \
    wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y aspnetcore-runtime-8.0 && \
    apt-get clean && \
    rm -rf /var/lib/apt/lists/* && \
    rm packages-microsoft-prod.deb

# Crear directorio para la aplicación
WORKDIR /app

# Copiar aplicación compilada
COPY --from=build /app/publish .

# Crear script de inicio
RUN cat > /start.sh << 'EOF'
#!/bin/bash
set -e

echo "🚀 Iniciando BusTicketDemoApp con SQL Server Express..."

# Iniciar SQL Server en segundo plano
echo "📦 Iniciando SQL Server Express..."
/opt/mssql/bin/sqlservr &

# Esperar a que SQL Server esté listo
echo "⏳ Esperando que SQL Server esté listo..."
for i in {1..60}; do
    if /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "SELECT 1" > /dev/null 2>&1; then
        echo "✅ SQL Server está listo!"
        break
    fi
    echo "⏳ Intento $i/60 - Esperando SQL Server..."
    sleep 5
done

# Crear la base de datos
echo "🗄️ Creando base de datos BusticketDb..."
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -Q "
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BusticketDb')
BEGIN
    CREATE DATABASE [BusticketDb];
    PRINT 'Base de datos BusticketDb creada exitosamente';
END
"

# Iniciar la aplicación .NET
echo "🌐 Iniciando aplicación .NET..."
cd /app
exec dotnet BusTicketDemoApp.dll
EOF

RUN chmod +x /start.sh

# Exponer puertos
EXPOSE 1433 80

# Health check
HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
    CMD curl -f http://localhost:80/api/BusBooking/GetAllBusLocations || exit 1

# Comando de inicio
CMD ["/start.sh"]
