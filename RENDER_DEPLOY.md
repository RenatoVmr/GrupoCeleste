# Deploy en Render con Docker

Este archivo contiene las instrucciones para desplegar GrupoCeleste en Render usando Docker.

## 🚀 Configuración en Render

### 1. Configurar el servicio en Render

1. Conecta tu repositorio GitHub a Render
2. Crea un nuevo **Web Service**
3. Configura los siguientes valores:

**Configuración básica:**
- **Build Command**: (vacío - se usa Dockerfile)
- **Start Command**: (vacío - se usa Dockerfile)
- **Dockerfile Path**: `./Dockerfile`
- **Docker Context Directory**: `./`

### 2. Variables de entorno requeridas

Configura estas variables de entorno en Render:

```bash
# ASP.NET Core
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:8080
PORT=8080

# Base de datos (SQLite persistente)
DATABASE_URL=Data Source=/app/Data/GrupoCeleste.db

# MercadoPago (configurar con tus credenciales)
MERCADOPAGO_ACCESS_TOKEN=tu_access_token_aqui
MERCADOPAGO_PUBLIC_KEY=tu_public_key_aqui
MERCADOPAGO_WEBHOOK_SECRET=tu_webhook_secret_aqui

# Configuración de logs
Logging__LogLevel__Default=Warning
Logging__LogLevel__Microsoft=Warning
```

### 3. Configuración de volumen persistente

Para mantener la base de datos SQLite entre deployments:

1. En la configuración de tu servicio en Render
2. Ve a la sección **Environment**
3. Agrega un **Persistent Disk**:
   - **Mount Path**: `/app/Data`
   - **Size**: 1GB (o el tamaño que necesites)

### 4. Puerto y Networking

- **Puerto interno**: 8080 (configurado en Dockerfile)
- **Puerto público**: Automático por Render
- **Health Check**: `GET /health` (configurado en la aplicación)

## 🔧 Comandos útiles para desarrollo

### Construcción local
```bash
docker build -t grupoceleste .
docker run -p 8080:8080 grupoceleste
```

### Con docker-compose
```bash
docker-compose up --build
```

### Limpieza
```bash
docker system prune -a
```

## 📝 Notas importantes

1. **Base de datos**: Usa SQLite con volumen persistente
2. **Variables de entorno**: Configurar en Render Dashboard
3. **SSL/HTTPS**: Manejado automáticamente por Render
4. **Logs**: Configurados para producción (nivel Warning)
5. **Health Check**: Endpoint `/health` disponible

## 🔑 Credenciales por defecto

Una vez deployado, las credenciales de administrador son:
- **Email**: admin@cineverse.com
- **Contraseña**: Admin123!

## 📚 Recursos

- [Render Documentation](https://render.com/docs)
- [ASP.NET Core Docker](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/docker/)
- [MercadoPago API](https://www.mercadopago.com.ar/developers)