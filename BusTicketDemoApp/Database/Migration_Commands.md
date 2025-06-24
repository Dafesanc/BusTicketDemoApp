# Entity Framework Core - Comandos de Migración

## 📋 Comandos Básicos de Migración

### Crear una nueva migración
```bash
dotnet ef migrations add NombreDeLaMigracion
```

### Aplicar migraciones a la base de datos
```bash
dotnet ef database update
```

### Aplicar una migración específica
```bash
dotnet ef database update NombreDeLaMigracion
```

### Revertir a una migración anterior
```bash
dotnet ef database update MigracionAnterior
```

### Eliminar la última migración (si no se ha aplicado)
```bash
dotnet ef migrations remove
```

### Ver el SQL que generará una migración
```bash
dotnet ef migrations script
```

### Ver el SQL desde una migración específica
```bash
dotnet ef migrations script MigracionInicial MigracionFinal
```

### Crear script SQL para toda la base de datos
```bash
dotnet ef dbcontext script
```

### Ver el estado de las migraciones
```bash
dotnet ef migrations list
```

### Eliminar la base de datos completamente
```bash
dotnet ef database drop
```

## 🔧 Comandos Avanzados

### Crear migración con nombre específico para un DbContext
```bash
dotnet ef migrations add InitialCreate --context BusTicketDbContext
```

### Aplicar migraciones en producción (con archivo)
```bash
dotnet ef migrations script --output migrations.sql
```

### Ver información del DbContext
```bash
dotnet ef dbcontext info
```

### Generar código del DbContext desde BD existente (Scaffold)
```bash
dotnet ef dbcontext scaffold "ConnectionString" Microsoft.EntityFrameworkCore.SqlServer
```

## 📊 Estructura de tu Base de Datos

### Tablas creadas:
- **Users**: Usuarios del sistema (Admin, Customer, Vendor)
- **Vendors**: Proveedores de servicios de autobús
- **BusLocations**: Ubicaciones/ciudades
- **BusSchedules**: Horarios y rutas de autobuses
- **BusBookings**: Reservas de boletos
- **BusBookingPassengers**: Pasajeros por reserva

### Relaciones:
- BusBookings → BusSchedules (FK: ScheduleId)
- BusBookingPassengers → BusBookings (FK: BookingId)

### Índices creados para optimización:
- Índice único en BusLocations.Code
- Índice único en Users.UserName y Users.EmailId
- Índice compuesto en BusSchedules para búsquedas por ruta y fecha
- Índice único en BusBookingPassengers para BookingId + SeatNo

## 🚀 Buenas Prácticas

1. **Siempre hacer backup** antes de aplicar migraciones en producción
2. **Revisar el SQL generado** antes de aplicar
3. **Usar nombres descriptivos** para las migraciones
4. **No modificar migraciones ya aplicadas** en producción
5. **Usar transacciones** para operaciones críticas

## 🗄️ Tu Conexión a Base de Datos

La aplicación está configurada para conectarse a:
- **Servidor**: DSKTOP_DSAMCJEZ\\SQLEXPRESS
- **Base de Datos**: BusticketDb
- **Autenticación**: SQL Server Authentication (sa)

Para verificar que todo funciona, puedes:
1. Abrir SQL Server Management Studio
2. Conectarte a tu instancia
3. Ver la base de datos "BusticketDb"
4. Explorar las tablas creadas

## 📱 Endpoints de tu API

Tu API ya está funcionando en http://localhost:5287 con todos los endpoints:

### Autenticación
- POST `/api/BusBooking/login`

### Usuarios
- GET `/api/BusBooking/GetAllUsers`
- POST `/api/BusBooking/AddNewUser`
- POST `/api/BusBooking/UpdateUser`

### Búsqueda y Horarios
- GET `/api/BusBooking/searchBus`
- GET `/api/BusBooking/GetBusScheduleById`

### Reservas
- POST `/api/BusBooking/PostBusBooking`
- GET `/api/BusBooking/GetAllBusBookings`
- GET `/api/BusBooking/getBookedSeats`

### Administración
- PUT `/api/BusBooking/PutBusLocation`
- GET `/api/BusBooking/GetAllBusLocations`
- GET `/api/BusBooking/GetAllVendors`
- GET `/api/BusBooking/GetAllBusSchedules`
