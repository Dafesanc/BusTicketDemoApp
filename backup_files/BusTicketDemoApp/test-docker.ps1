# 🧪 Script de Pruebas Local con Docker

# Construir la imagen Docker
Write-Host "🏗️ Construyendo imagen Docker..." -ForegroundColor Green
docker build -t bus-ticket-api .

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al construir la imagen Docker" -ForegroundColor Red
    exit 1
}

# Detener y remover contenedor existente si existe
Write-Host "🧹 Limpiando contenedores anteriores..." -ForegroundColor Yellow
docker stop bus-ticket-container 2>$null
docker rm bus-ticket-container 2>$null

# Ejecutar el contenedor
Write-Host "🚀 Iniciando contenedor..." -ForegroundColor Green
docker run -d `
    --name bus-ticket-container `
    -p 8080:80 `
    -p 1433:1433 `
    -e ASPNETCORE_ENVIRONMENT=Production `
    bus-ticket-api

if ($LASTEXITCODE -ne 0) {
    Write-Host "❌ Error al iniciar el contenedor" -ForegroundColor Red
    exit 1
}

Write-Host "⏳ Esperando que la aplicación inicie (60 segundos)..." -ForegroundColor Cyan
Start-Sleep -Seconds 60

# Verificar que la aplicación esté funcionando
Write-Host "🔍 Verificando que la aplicación esté funcionando..." -ForegroundColor Cyan

try {
    $response = Invoke-RestMethod -Uri "http://localhost:8080/api/BusBooking/GetAllBusLocations" -Method Get
    Write-Host "✅ ¡API funcionando correctamente!" -ForegroundColor Green
    Write-Host "📍 Ubicaciones encontradas: $($response.Count)" -ForegroundColor Cyan
} catch {
    Write-Host "❌ Error al conectar con la API: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "📋 Mostrando logs del contenedor:" -ForegroundColor Yellow
    docker logs bus-ticket-container
}

Write-Host "`n🌐 URLs de acceso:" -ForegroundColor Magenta
Write-Host "   API: http://localhost:8080" -ForegroundColor White
Write-Host "   Swagger: http://localhost:8080/swagger" -ForegroundColor White

Write-Host "`n📋 Comandos útiles:" -ForegroundColor Magenta
Write-Host "   Ver logs: docker logs bus-ticket-container -f" -ForegroundColor White
Write-Host "   Detener: docker stop bus-ticket-container" -ForegroundColor White
Write-Host "   Limpiar: docker rm bus-ticket-container" -ForegroundColor White
