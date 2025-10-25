# GrupoCeleste - Despliegue en Render

Este documento explica cómo desplegar GrupoCeleste en Render usando Docker.

## 🚀 Despliegue en Render

### 1. Preparación

1. **Fork o clona** este repositorio en tu cuenta de GitHub
2. **Configura las variables de entorno** en Render (ver sección Variables de Entorno)

### 2. Despliegue Automático

1. Ve a [Render](https://render.com) y conecta tu cuenta de GitHub
2. Busca el repositorio `GrupoCeleste`
3. Haz clic en "Connect" y configura:
   - **Service Type**: Web Service
   - **Build Command**: `docker build -t grupoceleste .`
   - **Start Command**: `docker run -p $PORT:8080 grupoceleste`

### 3. Variables de Entorno

Configura estas variables en Render:

```bash
# Mercado Pago (requeridas)
MERCADOPAGO_PUBLIC_KEY=tu_public_key_aqui
MERCADOPAGO_ACCESS_TOKEN=tu_access_token_aqui
MERCADOPAGO_CLIENT_ID=tu_client_id_aqui
MERCADOPAGO_CLIENT_SECRET=tu_client_secret_aqui

# ASP.NET Core (automáticas)
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:$PORT
```

### 4. Base de Datos

La aplicación usa SQLite por defecto. Para producción, considera:

- **SQLite**: Funciona bien para cargas ligeras
- **PostgreSQL**: Para mayor escalabilidad (requiere cambios en el código)

### 5. Health Checks

La aplicación incluye health checks en `/health` para monitoreo.

## 🐳 Desarrollo Local con Docker

### Usando Docker Compose

```bash
# Copiar variables de entorno
cp .env.example .env

# Ejecutar con Docker Compose
docker-compose up --build
```

### Usando Docker directamente

```bash
# Construir imagen
docker build -t grupoceleste .

# Ejecutar contenedor
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_URLS=http://+:8080 \
  grupoceleste
```

## 🔧 Configuración de Producción

### URLs de Mercado Pago

Actualiza las URLs de retorno en `Services/MercadoPagoService.cs`:

```csharp
back_urls = new
{
    success = "https://tu-app.render.com/Payment/Success",
    failure = "https://tu-app.render.com/Payment/Failure",
    pending = "https://tu-app.render.com/Payment/Pending"
}
```

### Base de Datos

Para producción con mayor carga, considera migrar a PostgreSQL:

1. Instala el paquete `Npgsql.EntityFrameworkCore.PostgreSQL`
2. Actualiza la cadena de conexión
3. Ejecuta las migraciones

## 📊 Monitoreo

- **Health Check**: `https://tu-app.render.com/health`
- **Logs**: Disponibles en el dashboard de Render
- **Métricas**: Configura alertas para uptime y rendimiento

## 🐛 Solución de Problemas

### Error de Build
- Verifica que todas las dependencias estén en el Dockerfile
- Revisa los logs de build en Render

### Error de Base de Datos
- Asegúrate de que el directorio `/app/data` tenga permisos de escritura
- Verifica la cadena de conexión

### Error de Mercado Pago
- Confirma que las credenciales sean válidas
- Verifica que las URLs de retorno estén correctas

## 📝 Notas Importantes

- La aplicación está optimizada para contenedores
- Incluye health checks para monitoreo automático
- Usa SQLite por defecto (adecuado para desarrollo y cargas ligeras)
- Todas las credenciales sensibles están en variables de entorno

## 🤝 Soporte

Para problemas específicos de despliegue, revisa:
- [Documentación de Render](https://docs.render.com/)
- [Documentación de ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/)
- [Documentación de Mercado Pago](https://www.mercadopago.com.ar/developers/es/)