# Agents

## Project

ASP.NET MVC 10.0 condominium administration app (university project). SQL Server + EF Core, Razor views, cookie auth.

## Solution structure

`AdministracionConsorcios.slnx` — 6 projects, strict dependency layers:

```
Model/          — Domain entities (zero dependencies). Start here to understand the data model.
DTOs/           — ViewModels and DTOs (zero dependencies).
Data/           — EF Core DbContext + migrations (→ Model).
Services/       — Business logic + interfaces (→ Data, DTOs, Model).
AdministracionConsorcios/ — MVC web app (→ all above). This is the only runnable project.
Servicio/       — Legacy early draft of ConsorcioService. NOT referenced by the web app. Ignore it.
Services.Tests/ — xUnit tests (→ Data, Services).
```

Dependency rule: lower layers must never reference upper layers. Model knows nothing; Data knows Model; Services knows Data+Model; the web app knows everything.

## Commands

```bash
# Build (from repo root)
dotnet build

# Run the web app
dotnet run --project AdministracionConsorcios

# Run all tests
dotnet test

# Run a single test class
dotnet test --filter "FullyQualifiedName~Services.Tests.UsuarioServiceTests"

# EF Core migrations (from repo root, target the web project)
dotnet ef migrations add <MigrationName> --project Data --startup-project AdministracionConsorcios
dotnet ef database update --project Data --startup-project AdministracionConsorcios
```

## Database

SQL Server, connection string in `AdministracionConsorcios/appsettings.json` under `ConnectionStrings:DefaultConnection`. Default points to `localhost` with Windows auth (`Trusted_Connection`).

`appsettings.Development.json` is gitignored — it exists locally for secrets. `appsettings.json` contains a committed SendGrid API key (treat it as non-sensitive for this project context).

## Architecture patterns

- **DI**: all services registered as `Scoped` in `Program.cs`. New services must be added there.
- **Auth**: Cookie authentication. Claims include `ClaimTypes.Name` (email) and `UsuarioId` (int). Get current user ID via `int.Parse(User.FindFirst("UsuarioId").Value)`.
- **Views**: Razor, two shared layouts — `_LogeadoLayout` (authenticated), `_AnonimoLayout` (anonymous). Controllers under `[Authorize]` use the logged-in layout automatically.
- **CRUD pattern**: every entity (Consorcio, Unidad, Gasto, Sum, ReservaSum, Notificacion) has Controller → Service → Service Interface → DbContext. Follow the existing pattern when adding new entities.
- **Geocoding**: `GeocodingService` calls Nominatim (OpenStreetMap) to geocode addresses on Consorcio create/edit. Uses `IHttpClientFactory` via `AddHttpClient<IGeocodingService, GeocodingService>()`.
- **Email**: `EmailService` uses SendGrid. Registered directly (not through an interface).

## Testing

xUnit + InMemory EF Core database. Tests instantiate `ConsorcioContext` with `UseInMemoryDatabase(Guid.NewGuid().ToString())` — each test gets an isolated database. Moq is available but tests currently wire real service implementations directly.

No CI pipeline exists. No pre-commit hooks. Run `dotnet test` manually before pushing.

## Conventions

- Namespaces match project names (`Model`, `Data`, `Services`, `DTOs.ViewModels`, `AdministracionConsorcios.Controllers`).
- Service interfaces live in `Services/Interfaces/` — naming convention: `I{Entity}Service`.
- ViewModels live in `DTOs/ViewModels/` — naming convention: `{Entity}ViewModel`.
- Spanish naming for business concepts (Consorcio, Gasto, Expensa, Unidad, ReservaSum, Notificacion, Provincia).
- Nullable reference types enabled across all projects.
- `TempData["Exito"]` and `TempData["Error"]` for flash messages (see `_Mensajes.cshtml` partial).

## Gotchas

- `Servicio/` project is dead code from an earlier iteration — it is not in the dependency graph of the running app. Do not add references to it.
- The `.slnx` format is the newer XML solution format (not the classic `.sln`). Use `dotnet build` or open with a modern IDE; older tools may not recognize it.
- EF migrations live in `Data/Migrations/`. There are 7 migrations. Always target `--project Data --startup-project AdministracionConsorcios` when running dotnet-ef commands.
- `appsettings.Development.json` is gitignored — if you need to override connection strings or API keys locally, create it manually.
- The web project uses `UserSecretsId` — `dotnet user-secrets` works for local secret management.
