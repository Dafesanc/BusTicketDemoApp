# 🚀 Guía de Despliegue en Fly.io

## Prerrequisitos

1. **Instalar Fly.io CLI**
   ```bash
   # Windows (PowerShell)
   iwr https://fly.io/install.ps1 -useb | iex
   
   # MacOS/Linux
   curl -L https://fly.io/install.sh | sh
   ```

2. **Crear cuenta en Fly.io** (Plan gratuito incluye 3 aplicaciones)
   ```bash
   fly auth signup
   # o si ya tienes cuenta:
   fly auth login
   ```

## Pasos de Despliegue

### 1. Navegar al directorio de la aplicación
```bash
cd "c:\Users\super\Documents\Proyectos full stack\Angular&.NET\BusTicketBackEnd\BusTicketDemoApp"
```

### 2. Inicializar proyecto Fly.io
```bash
fly launch --no-deploy
```
- Selecciona un nombre único para tu app (ej: `bus-ticket-api-2025`)
- Selecciona la región más cercana (ej: `mia` para Miami)
- No configures PostgreSQL (usaremos SQL Server embebido)
- No despliegues aún

### 3. Crear volumen persistente para la base de datos
```bash
fly volumes create bus_ticket_data --region mia --size 1
```

### 4. Desplegar la aplicación
```bash
fly deploy
```

### 5. Verificar el despliegue
```bash
# Ver logs en tiempo real
fly logs

# Verificar estatus
fly status

# Abrir la aplicación en el navegador
fly open
```

## URLs de tu API

Una vez desplegada, tu API estará disponible en:
- **Aplicación principal**: `https://tu-app-name.fly.dev`
- **Swagger UI**: `https://tu-app-name.fly.dev/swagger`
- **Health Check**: `https://tu-app-name.fly.dev/api/BusBooking/GetAllBusLocations`

## Endpoints de Prueba

### 🔐 Login
```bash
curl -X POST "https://tu-app-name.fly.dev/api/BusBooking/login" \
  -H "Content-Type: application/json" \
  -d '{
    "userName": "admin",
    "password": "admin123"
  }'
```

### 🔍 Buscar Autobuses
```bash
curl "https://tu-app-name.fly.dev/api/BusBooking/searchBus?fromLocation=1&toLocation=2&travelDate=2025-06-24"
```

### 📍 Ver Ubicaciones
```bash
curl "https://tu-app-name.fly.dev/api/BusBooking/GetAllBusLocations"
```

## Comandos Útiles

### Ver logs de la aplicación
```bash
fly logs --app tu-app-name
```

### Conectarse al contenedor
```bash
fly ssh console --app tu-app-name
```

### Escalar la aplicación
```bash
fly scale count 1 --app tu-app-name
```

### Actualizar la aplicación
```bash
# Después de hacer cambios al código
fly deploy
```

### Ver métricas
```bash
fly dashboard
```

## Solución de Problemas

### Si la aplicación no inicia:
1. Verificar logs: `fly logs`
2. Verificar que SQL Server haya iniciado correctamente
3. Verificar conexión a la base de datos

### Si hay problemas de memoria:
```bash
# Escalar a una máquina más grande
fly scale vm shared-cpu-2x --app tu-app-name
```

### Para debugging:
```bash
# Conectarse al contenedor
fly ssh console

# Verificar SQL Server
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P BusTicket2025!

# Verificar aplicación .NET
ps aux | grep dotnet
```

## Costos

El plan gratuito de Fly.io incluye:
- 3 aplicaciones pequeñas
- 160GB de transferencia de datos
- Shared CPU y memoria limitada

Para mayor rendimiento, considera upgrading a plan pagado.

## Seguridad

En producción, considera:
1. Cambiar la contraseña de SA de SQL Server
2. Implementar JWT authentication real
3. Configurar HTTPS certificates personalizados
4. Configurar rate limiting
