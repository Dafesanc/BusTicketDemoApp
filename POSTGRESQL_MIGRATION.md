# Migración a PostgreSQL y Despliegue en Render

## Cambios Realizados

### 1. Dependencias del Proyecto
- ✅ Reemplazado `Microsoft.EntityFrameworkCore.SqlServer` con `Npgsql.EntityFrameworkCore.PostgreSQL`
- ✅ Mantenido Entity Framework Core 8.0

### 2. Configuración de Conexión
- ✅ Actualizado `Program.cs` para soportar PostgreSQL
- ✅ Función `GetConnectionString()` que parsea `DATABASE_URL` de Render
- ✅ Configuración de conexión local para desarrollo

### 3. Configuración de Archivos
- ✅ `appsettings.json`: Conexión local PostgreSQL
- ✅ `appsettings.Production.json`: Preparado para variable de entorno
- ✅ `DbContext`: Configurado para PostgreSQL

### 4. Migraciones
- ✅ Eliminadas migraciones de SQL Server
- ✅ Creada nueva migración inicial para PostgreSQL

## Configuración en Render

### 1. Base de Datos PostgreSQL
1. En Render Dashboard, crear un nuevo PostgreSQL database
2. Copiar la URL de conexión externa (aparece como "External Database URL")
3. La URL será similar a: `postgres://username:password@host:port/database`

### 2. Web Service
1. Crear un nuevo Web Service conectado a tu repositorio GitHub
2. Configurar las siguientes variables de entorno:

```bash
DATABASE_URL=postgres://username:password@host:port/database
ASPNETCORE_ENVIRONMENT=Production
```

### 3. Configuración del Servicio
- **Build Command**: `dotnet build -c Release`
- **Start Command**: `dotnet BusTicketDemoApp.dll`
- **Port**: 10000 (configurado automáticamente)

## Configuración Local para Desarrollo

### 1. Instalar PostgreSQL
```bash
# Windows con Chocolatey
choco install postgresql

# O descargar desde https://www.postgresql.org/download/
```

### 2. Crear Base de Datos Local
```sql
-- Conectar a PostgreSQL como superusuario
CREATE DATABASE BusticketDb;
CREATE USER postgres WITH PASSWORD 'password';
GRANT ALL PRIVILEGES ON DATABASE BusticketDb TO postgres;
```

### 3. Actualizar Connection String Local
En `appsettings.json`, ajustar si es necesario:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=BusticketDb;Username=postgres;Password=password;"
  }
}
```

### 4. Aplicar Migraciones
```bash
dotnet ef database update
```

## GitHub Actions para CI/CD

Crear `.github/workflows/deploy.yml`:

```yaml
name: Deploy to Render

on:
  push:
    branches: [ main ]

jobs:
  deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: 8.0.x
        
    - name: Restore dependencies
      run: dotnet restore ./BusTicketDemoApp
      
    - name: Build
      run: dotnet build ./BusTicketDemoApp --no-restore -c Release
      
    - name: Test
      run: dotnet test ./BusTicketDemoApp --no-build -c Release --verbosity normal
      
    # Render se encarga del deployment automático cuando hay push a main
```

## Verificación del Despliegue

### 1. Endpoints para Probar
- `GET /api/busbooking/locations` - Listar ubicaciones
- `GET /api/busbooking/schedules` - Listar horarios
- `GET /swagger` - Documentación API

### 2. Logs en Render
- Verificar que las migraciones se aplican correctamente
- Verificar que la conexión a PostgreSQL es exitosa
- Confirmar que el seeding de datos funciona

## Comandos Útiles

### Entity Framework
```bash
# Crear nueva migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir migración
dotnet ef database update PreviousMigrationName

# Eliminar última migración
dotnet ef migrations remove
```

### Docker Local
```bash
# Construir imagen
docker build -t busticket-api .

# Ejecutar con PostgreSQL
docker run -p 10000:10000 -e DATABASE_URL="postgres://user:pass@host:5432/db" busticket-api
```

## Solución de Problemas

### Error de Conexión
- Verificar que `DATABASE_URL` está configurada correctamente
- Verificar que la base de datos PostgreSQL está accesible
- Revisar logs de Render para errores específicos

### Error de Migración
- Verificar que las migraciones están incluidas en el proyecto
- Confirmar que Entity Framework está configurado correctamente
- Revisar que no hay conflictos de esquema

### Error de Despliegue
- Verificar que el Dockerfile funciona localmente
- Confirmar que todas las dependencias están en el .csproj
- Revisar los logs de build en Render
```
