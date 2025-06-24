-- Script de Base de Datos para BusTicketDemoApp
-- Generado automáticamente por Entity Framework Core
-- Fecha: 2025-06-22

-- Crear base de datos (si no existe)
-- USE master;
-- IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BusticketDb')
-- CREATE DATABASE [BusticketDb];
-- GO
-- USE [BusticketDb];
-- GO

-- Tabla: BusLocations (Ubicaciones de autobuses)
CREATE TABLE [BusLocations] (
    [LocationId] int NOT NULL IDENTITY,
    [LocationName] nvarchar(100) NOT NULL,
    [Code] nvarchar(10) NOT NULL,
    CONSTRAINT [PK_BusLocations] PRIMARY KEY ([LocationId])
);
GO

-- Tabla: BusSchedules (Horarios de autobuses)
CREATE TABLE [BusSchedules] (
    [ScheduleId] int NOT NULL IDENTITY,
    [VendorId] int NOT NULL,
    [BusName] nvarchar(100) NOT NULL,
    [BusVehicleNo] nvarchar(20) NOT NULL,
    [FromLocation] int NOT NULL,
    [ToLocation] int NOT NULL,
    [DepartureTime] datetime2 NOT NULL,
    [ArrivalTime] datetime2 NOT NULL,
    [ScheduleDate] datetime2 NOT NULL,
    [Price] decimal(10,2) NOT NULL,
    [TotalSeats] int NOT NULL,
    CONSTRAINT [PK_BusSchedules] PRIMARY KEY ([ScheduleId])
);
GO

-- Tabla: Users (Usuarios del sistema)
CREATE TABLE [Users] (
    [UserId] int NOT NULL IDENTITY,
    [UserName] nvarchar(50) NOT NULL,
    [EmailId] nvarchar(100) NOT NULL,
    [FullName] nvarchar(100) NOT NULL,
    [Role] nvarchar(20) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    [ProjectName] nvarchar(50) NOT NULL,
    [RefreshToken] nvarchar(max) NULL,
    [RefreshTokenExpiryTime] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([UserId])
);
GO

-- Tabla: Vendors (Proveedores de autobuses)
CREATE TABLE [Vendors] (
    [VendorId] int NOT NULL IDENTITY,
    [VendorName] nvarchar(100) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Vendors] PRIMARY KEY ([VendorId])
);
GO

-- Tabla: BusBookings (Reservas de autobuses)
CREATE TABLE [BusBookings] (
    [BookingId] int NOT NULL IDENTITY,
    [CustId] int NOT NULL,
    [BookingDate] datetime2 NOT NULL,
    [ScheduleId] int NOT NULL,
    CONSTRAINT [PK_BusBookings] PRIMARY KEY ([BookingId]),
    CONSTRAINT [FK_BusBookings_BusSchedules_ScheduleId] FOREIGN KEY ([ScheduleId]) REFERENCES [BusSchedules] ([ScheduleId]) ON DELETE CASCADE
);
GO

-- Tabla: BusBookingPassengers (Pasajeros de las reservas)
CREATE TABLE [BusBookingPassengers] (
    [PassengerId] int NOT NULL IDENTITY,
    [BookingId] int NOT NULL,
    [PassengerName] nvarchar(100) NOT NULL,
    [Age] int NOT NULL,
    [Gender] nvarchar(10) NOT NULL,
    [SeatNo] int NOT NULL,
    CONSTRAINT [PK_BusBookingPassengers] PRIMARY KEY ([PassengerId]),
    CONSTRAINT [FK_BusBookingPassengers_BusBookings_BookingId] FOREIGN KEY ([BookingId]) REFERENCES [BusBookings] ([BookingId]) ON DELETE CASCADE
);
GO

-- Datos iniciales: BusLocations
SET IDENTITY_INSERT [BusLocations] ON;
INSERT INTO [BusLocations] ([LocationId], [Code], [LocationName])
VALUES 
(1, N'CDMX', N'Ciudad de México'),
(2, N'GDL', N'Guadalajara'),
(3, N'MTY', N'Monterrey'),
(4, N'PUE', N'Puebla'),
(5, N'GYE', N'Guayaquil');
(6, N'UIO', N'Quito');
(7, N'CUE', N'Cuenca');
(8, N'MEC', N'Manta');
(9, N'ESM', N'Esmeraldas');


SET IDENTITY_INSERT [BusLocations] OFF;
GO

-- Datos iniciales: Vendors
SET IDENTITY_INSERT [Vendors] ON;
INSERT INTO [Vendors] ([VendorId], [CreatedDate], [Email], [Phone], [VendorName])
VALUES 
(1, '2025-01-01T00:00:00.0000000', N'info@autobusdelnorte.com', N'+52-55-1234-5678', N'Autobuses del Norte'),
(2, '2025-01-01T00:00:00.0000000', N'contacto@primeraplus.com', N'+52-33-9876-5432', N'Primera Plus');
(3, '2025-01-01T00:00:00.0000000', N'contacto@primetravel.com', N'+593-33-987-5467', N'PrimeTravel Ecuador');

(4, '2025-01-01T00:00:00.0000000', N'contacto@transportesecuador.com', N'+593-33-987-6565', N'Transportes Ecuador');

SET IDENTITY_INSERT [Vendors] OFF;
GO

-- Datos iniciales: Users
SET IDENTITY_INSERT [Users] ON;
INSERT INTO [Users] ([UserId], [CreatedDate], [EmailId], [FullName], [Password], [ProjectName], [RefreshToken], [RefreshTokenExpiryTime], [Role], [UserName])
VALUES 
(1, '2025-01-01T00:00:00.0000000', N'admin@busticket.com', N'Administrador del Sistema', N'admin123', N'BusTicketDemo', NULL, NULL, N'Admin', N'admin'),
(2, '2025-01-01T00:00:00.0000000', N'customer1@email.com', N'Juan Pérez', N'customer123', N'BusTicketDemo', NULL, NULL, N'Customer', N'customer1'),
(3, '2025-01-01T00:00:00.0000000', N'vendor1@email.com', N'María González', N'vendor123', N'BusTicketDemo', NULL, NULL, N'Vendor', N'vendor1');
(4, '2025-05-05T00:00:00.0000000', 'dafesanc12@gmail.com', N'Daniel Sanchez Cordova', '12345', N'BusTicketDemo', NULL, NULL, N'Customer', N'danielsc');
SET IDENTITY_INSERT [Users] OFF;
GO

-- Datos iniciales: BusSchedules
SET IDENTITY_INSERT [BusSchedules] ON;
INSERT INTO [BusSchedules] ([ScheduleId], [ArrivalTime], [BusName], [BusVehicleNo], [DepartureTime], [FromLocation], [Price], [ScheduleDate], [ToLocation], [TotalSeats], [VendorId])
VALUES 
(1, '2025-06-23T14:00:00.0000000', N'Express CDMX-GDL', N'ADN-001', '2025-06-23T08:00:00.0000000', 1, 450.0, '2025-06-23T00:00:00.0000000', 2, 42, 1),
(2, '2025-06-23T20:00:00.0000000', N'Ejecutivo CDMX-MTY', N'ADN-002', '2025-06-23T10:00:00.0000000', 1, 650.0, '2025-06-23T00:00:00.0000000', 3, 38, 1),
(3, '2025-06-24T16:00:00.0000000', N'Primera Plus GDL-CUN', N'PP-101', '2025-06-23T22:00:00.0000000', 2, 980.0, '2025-06-23T00:00:00.0000000', 5, 40, 2),
(4, '2025-06-24T18:00:00.0000000', N'PrimeTravel Ecuador GDL-MTY', N'PP-1028', '2025-06-23T12:00:00.000', 5, 750.50, '2025-06-24T00:00:00.0000000', 6, 40, 3),
(5, '2025-06-24T20:00:00.0000000', N'Transportes Ecuador GYE-Quito', N'TE-001', '2025-06-23T16:00:00.0000000', 6, 300.0, '2025-06-23T00:00:00.0000000', 7, 45, 4);
SET IDENTITY_INSERT [BusSchedules] OFF;
GO

-- Crear índices para optimizar consultas
CREATE UNIQUE INDEX [IX_BusBookingPassengers_BookingId_SeatNo] ON [BusBookingPassengers] ([BookingId], [SeatNo]);
GO

CREATE INDEX [IX_BusBookings_ScheduleId] ON [BusBookings] ([ScheduleId]);
GO

CREATE UNIQUE INDEX [IX_BusLocations_Code] ON [BusLocations] ([Code]);
GO

CREATE INDEX [IX_BusSchedules_FromLocation_ToLocation_ScheduleDate] ON [BusSchedules] ([FromLocation], [ToLocation], [ScheduleDate]);
GO

CREATE UNIQUE INDEX [IX_Users_EmailId] ON [Users] ([EmailId]);
GO

CREATE UNIQUE INDEX [IX_Users_UserName] ON [Users] ([UserName]);
GO

-- Fin del script
