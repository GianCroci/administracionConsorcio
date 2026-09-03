# Administración de Consorcios

Aplicación web para la gestión de consorcios (condominios), desarrollada como trabajo práctico universitario con **ASP.NET Core MVC** y **Entity Framework Core**.

## Funcionalidades

- **Consorcios**: alta, edición y geolocalización automática de direcciones (Nominatim/OpenStreetMap).
- **Unidades**: administración de unidades funcionales por consorcio.
- **Gastos y Expensas**: liquidación de expensas con tablas dinámicas (DataTables vía Ajax/JSON).
- **Reservas de espacios comunes (Sum)**.
- **Notificaciones**: envío de mails a través de SendGrid.
- **Autenticación** basada en cookies, con roles y layouts separados para usuarios logueados y anónimos.

## Stack técnico

- ASP.NET Core MVC (.NET 10.0)
- Entity Framework Core (Code First) + SQL Server
- Razor Views
- xUnit + EF Core InMemory para testing
- SendGrid (envío de mails) y Nominatim (geocoding)

## Estructura del proyecto

La solución (`AdministracionConsorcios.slnx`) está organizada en capas, con dependencias estrictas de abajo hacia arriba:

```
Model/                      Entidades de dominio (sin dependencias)
DTOs/                       ViewModels y DTOs (sin dependencias)
Data/                       DbContext de EF Core + migraciones (→ Model)
Services/                   Lógica de negocio e interfaces (→ Data, DTOs, Model)
AdministracionConsorcios/   Aplicación web MVC (→ todo lo anterior). Único proyecto ejecutable.
Services.Tests/             Tests con xUnit (→ Data, Services)
Servicio/                   Borrador legacy de ConsorcioService. No se usa (ignorar).
```

## Requisitos previos

- [.NET SDK 10.0](https://dotnet.microsoft.com/download)
- SQL Server (local o remoto)

## Cómo correrlo

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/GianCroci/administracionConsorcio.git
   cd administracionConsorcio
   ```

2. Configurar la cadena de conexión en `AdministracionConsorcios/appsettings.json` (clave `ConnectionStrings:DefaultConnection`), o crear un `appsettings.Development.json` local para no versionar cambios.

3. Aplicar las migraciones de la base de datos:
   ```bash
   dotnet ef database update --project Data --startup-project AdministracionConsorcios
   ```

4. Ejecutar la aplicación:
   ```bash
   dotnet run --project AdministracionConsorcios
   ```

## Tests

```bash
dotnet test
```

Para correr una clase de test específica:

```bash
dotnet test --filter "FullyQualifiedName~Services.Tests.UsuarioServiceTests"
```

## Contexto

Este proyecto fue desarrollado como trabajo práctico de la materia Administración de Consorcios (UNLaM), aplicando patrones CRUD (Controller → Service → Interface → DbContext) y buenas prácticas de arquitectura en capas.
