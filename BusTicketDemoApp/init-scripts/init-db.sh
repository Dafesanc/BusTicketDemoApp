#!/bin/bash
set -e

# Esperar a que SQL Server esté completamente inicializado
echo "Esperando que SQL Server se inicialice completamente..."
sleep 45

# Crear la base de datos BusticketDb si no existe
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P BusTicket2025! -Q "
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'BusticketDb')
BEGIN
    CREATE DATABASE [BusticketDb];
    PRINT 'Base de datos BusticketDb creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Base de datos BusticketDb ya existe';
END
"

echo "Inicialización de base de datos completada."
