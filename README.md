# 🚌 BusTicketDemoApp - API REST para Gestión de Reservas de Autobuses

Una API REST completa desarrollada en **.NET 8.0** para gestionar reservas de autobuses con sistema de usuarios, proveedores, rutas y reservas.

## 🏗️ Arquitectura

- **Framework**: ASP.NET Core Web API (.NET 8.0)
- **Base de Datos**: SQL Server Express (embebida en Docker)
- **ORM**: Entity Framework Core 8.0
- **Documentación**: Swagger/OpenAPI
- **Deployment**: Docker + Fly.io

## 📊 Modelo de Datos

### Entidades Principales

#### 👤 **Users** - Sistema de Usuarios
```csharp
- UserId (PK)
- UserName (Unique)
- EmailId (Unique) 
- FullName
- Role (Admin, Customer, Vendor)
- Password
- CreatedDate
- RefreshToken & RefreshTokenExpiryTime
```

#### 🏢 **Vendors** - Proveedores de Autobuses
```csharp
- VendorId (PK)
- VendorName
- Email
- Phone
- CreatedDate
```

#### 📍 **BusLocations** - Ubicaciones/Rutas
```csharp
- LocationId (PK)
- LocationName
- Code (Unique) - ej: "CDMX", "GDL"
```

#### 🚌 **BusSchedules** - Horarios de Autobuses
```csharp
- ScheduleId (PK)
- VendorId (FK)
- BusName
- BusVehicleNo
- FromLocation (FK to BusLocations)
- ToLocation (FK to BusLocations)
- DepartureTime
- ArrivalTime
- ScheduleDate
- Price (decimal)
- TotalSeats
```

#### 🎫 **BusBookings** - Reservas
```csharp
- BookingId (PK)
- CustId (FK to Users)
- BookingDate
- ScheduleId (FK to BusSchedules)
```

#### 👥 **BusBookingPassengers** - Pasajeros
```csharp
- PassengerId (PK)
- BookingId (FK)
- PassengerName
- Age
- Gender
- SeatNo (Unique per booking)
```

## 🔌 API Endpoints

### 🔐 Autenticación
```http
POST /api/BusBooking/login
Content-Type: application/json

{
  "userName": "admin",
  "password": "admin123"
}
```

### 👥 Gestión de Usuarios
```http
GET    /api/BusBooking/GetAllUsers
POST   /api/BusBooking/AddNewUser
POST   /api/BusBooking/UpdateUser
```

### 🔍 Búsqueda y Horarios
```http
GET /api/BusBooking/searchBus?fromLocation=1&toLocation=2&travelDate=2025-06-24
GET /api/BusBooking/GetBusScheduleById?id=1
GET /api/BusBooking/GetAllBusSchedules
```

### 🎫 Sistema de Reservas
```http
POST /api/BusBooking/PostBusBooking
GET  /api/BusBooking/GetAllBusBookings?vendorId=1
GET  /api/BusBooking/getBookedSeats?shceduleId=1
```

### ⚙️ Administración
```http
GET /api/BusBooking/GetAllBusLocations
PUT /api/BusBooking/PutBusLocation?id=1
GET /api/BusBooking/GetAllVendors

# Endpoints de ayuda para testing
POST /api/BusBooking/AddBusLocation
POST /api/BusBooking/AddVendor
POST /api/BusBooking/AddBusSchedule
```

## 🌍 Datos de Ejemplo

### Ubicaciones Disponibles
- **CDMX** - Ciudad de México
- **GDL** - Guadalajara  
- **MTY** - Monterrey
- **PUE** - Puebla
- **GYE** - Guayaquil
- **UIO** - Quito
- **CUE** - Cuenca
- **MEC** - Manta
- **ESM** - Esmeraldas

### Proveedores
- **Autobuses del Norte** (México)
- **Primera Plus** (México)
- **PrimeTravel Ecuador**
- **Transportes Ecuador**

### Usuarios de Prueba
```json
{
  "admin": { "password": "admin123", "role": "Admin" },
  "customer1": { "password": "customer123", "role": "Customer" },
  "vendor1": { "password": "vendor123", "role": "Vendor" },
  "danielsc": { "password": "12345", "role": "Customer" }
}
```

## 🚀 Instalación y Desarrollo

### Prerrequisitos
- .NET 8.0 SDK
- SQL Server (Express o completo)

### Configuración Local
```bash
# Clonar el repositorio
git clone <repository-url>
cd BusTicketBackEnd/BusTicketDemoApp

# Restaurar dependencias
dotnet restore

# Configurar connection string en appsettings.json
# Aplicar migraciones
dotnet ef database update

# Ejecutar la aplicación
dotnet run
```

La API estará disponible en: `https://localhost:7287` o `http://localhost:5287`

### Base de Datos
- Las migraciones se aplican automáticamente al iniciar la aplicación
- Los datos de ejemplo se insertan mediante **Entity Framework Seeding**
- Schema SQL disponible en: `Database/BusTicketDatabase_Schema.sql`

## 🐳 Docker Deployment

### Construcción de la Imagen
```dockerfile
# Dockerfile incluye SQL Server Express + API
FROM mcr.microsoft.com/mssql/server:2022-latest AS sql-server
# ... configuración SQL Server

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS api
# ... configuración API .NET
```

### Despliegue en Fly.io
```bash
# Inicializar proyecto Fly.io
fly launch

# Configurar secretos
fly secrets set ConnectionStrings__DefaultConnection="Server=localhost;Database=BusticketDb;Integrated Security=true;TrustServerCertificate=true;"

# Desplegar
fly deploy
```

## 📱 Uso de la API

### Ejemplo: Búsqueda de Autobuses
```bash
curl -X GET "https://your-app.fly.dev/api/BusBooking/searchBus?fromLocation=1&toLocation=2&travelDate=2025-06-24"
```

### Ejemplo: Crear Reserva
```bash
curl -X POST "https://your-app.fly.dev/api/BusBooking/PostBusBooking" \
  -H "Content-Type: application/json" \
  -d '{
    "custId": 2,
    "bookingDate": "2025-06-24T00:00:00",
    "scheduleId": 1,
    "busBookingPassengers": [
      {
        "passengerName": "Juan Pérez",
        "age": 30,
        "gender": "Male",
        "seatNo": 1
      }
    ]
  }'
```

## 🛠️ Tecnologías Utilizadas

- **ASP.NET Core 8.0** - Framework web
- **Entity Framework Core 8.0** - ORM
- **SQL Server Express** - Base de datos
- **Swagger/OpenAPI** - Documentación de API
- **Docker** - Containerización
- **Fly.io** - Hosting cloud

## 📄 Licencia

Este proyecto está bajo la licencia MIT.

## 👨‍💻 Desarrollado por

Daniel Sanchez Cordova - [dafesanc12@gmail.com](mailto:dafesanc12@gmail.com)