# Deploy en Render con Docker

Este archivo contiene las instrucciones para desplegar GrupoCeleste en Render usando Docker.

## 🚀 Configuración en Render

### 1. Configurar el servicio en Render

#### Opción A: Manual
1. Conecta tu repositorio GitHub a Render
2. Crea un nuevo **Web Service**
3. Configura los siguientes valores:

**Configuración básica:**
- **Build Command**: (vacío - se usa Dockerfile)
- **Start Command**: (vacío - se usa Dockerfile)
- **Dockerfile Path**: `./Dockerfile`
- **Docker Context Directory**: `./`

#### Opción B: Usando render.yaml (Recomendado)
1. El archivo `render.yaml` está incluido en el repositorio
2. Render detectará automáticamente la configuración
3. Solo necesitas conectar el repositorio

### 2. Variables de entorno requeridas

#### Variables automáticas (NO configurar):
- `PORT` - Render la proporciona automáticamente

#### Variables que debes configurar:
```bash
# MercadoPago (configurar con tus credenciales)
MERCADOPAGO_ACCESS_TOKEN=tu_access_token_aqui
MERCADOPAGO_PUBLIC_KEY=tu_public_key_aqui
MERCADOPAGO_WEBHOOK_SECRET=tu_webhook_secret_aqui
```

#### Variables opcionales:
```bash
# Configuración de logs (ya configuradas por defecto)
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

- **Puerto interno**: Dinámico (usa variable PORT de Render)
- **Puerto público**: Automático por Render
- **Health Check**: `GET /health` (configurado en la aplicación)

## 🔧 Troubleshooting

### Error "Address already in use"
Si ves este error:
```
Failed to bind to address http://0.0.0.0:8080: address already in use
```

**Causas comunes:**
1. **Caché de Render**: Render puede estar usando una versión anterior del código
2. **Variables de entorno incorrectas**: No configurar `PORT` manualmente
3. **Configuración duplicada**: Múltiples formas de configurar el puerto

**Soluciones:**
1. **Hacer redeploy manual** en Render Dashboard
2. **Verificar que NO tienes** `PORT` en variables de entorno
3. **Limpiar caché** haciendo un nuevo commit con cambio menor

### Logs útiles
Para depurar, revisa los logs en Render Dashboard:
- Ve a tu servicio → **Logs**
- Busca errores específicos de ASP.NET Core
- Verifica que las variables de entorno estén cargadas

## 🔧 Comandos útiles para desarrollo

### Construcción local
```bash
docker build -t grupoceleste .
docker run -p 8080:8080 -e PORT=8080 grupoceleste
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