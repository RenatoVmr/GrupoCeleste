# Usar la imagen oficial de .NET 9.0 para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app

# Usar la imagen del SDK para compilar
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar el archivo de proyecto y restaurar dependencias
COPY ["GrupoCeleste.csproj", "./"]
RUN dotnet restore "GrupoCeleste.csproj"

# Copiar todo el código fuente y compilar
COPY . .
RUN dotnet build "GrupoCeleste.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "GrupoCeleste.csproj" -c Release -o /app/publish

# Configurar la imagen final
FROM base AS final
WORKDIR /app

# Instalar herramientas necesarias para SQLite
RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

# Copiar la aplicación publicada
COPY --from=publish /app/publish .

# Crear directorio para la base de datos
RUN mkdir -p /app/Data

# Configurar variables de entorno para producción
ENV ASPNETCORE_ENVIRONMENT=Production

# Crear usuario no-root para seguridad
RUN groupadd -r appuser && useradd -r -g appuser appuser
RUN chown -R appuser:appuser /app
USER appuser

ENTRYPOINT ["dotnet", "GrupoCeleste.dll"]