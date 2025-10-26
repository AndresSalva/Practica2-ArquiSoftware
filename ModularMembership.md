# Servicio de Membresías (Modular)

## Objetivo
Centralizar la lógica de membresías y suscripciones de usuarios (DetailsUser) en un módulo reutilizable (`ServiceMembership`) que pueda integrarse en otros servicios sin depender de implementaciones internas del proyecto web.

## Estructura del módulo
```
ServiceMembership/
 ├── Application/
 │   ├── Interfaces/IDetailUserService.cs
 │   ├── Interfaces/IMembershipService.cs
 │   ├── Services/DetailUserService.cs
 │   └── Services/MembershipService.cs
 ├── Domain/
 │   ├── Entities/DetailsUser.cs
 │   ├── Entities/Membership.cs
 │   ├── Ports/IDetailUserRepository.cs
 │   └── Ports/IMembershipRepository.cs
 ├── Infrastructure/
 │   ├── DependencyInjection/MembershipModuleServiceCollectionExtensions.cs
 │   ├── Persistence/DetailUserRepository.cs
 │   ├── Persistence/MembershipRepository.cs
 │   └── Providers/IMembershipConnectionProvider.cs
 └── ServiceMembership.csproj
```

### Capas principales
- **Domain**: define `Membership`, `DetailsUser` y sus contratos (`IMembershipRepository`, `IDetailUserRepository`) desacoplados del sitio web.
- **Application**: contiene `IMembershipService` e `IDetailUserService`, junto con las implementaciones que orquestan la lógica de negocio.
- **Infrastructure**: repositorios Dapper (`MembershipRepository`, `DetailUserRepository`), proveedor de cadena de conexión y la extensión `AddMembershipModule` para registrar todo vía DI.

## Integración con GYMPT
- `GYMPT/GYMPT.csproj` referencia `ServiceMembership.csproj`.
- `Program.cs` registra el módulo reutilizando `ConnectionStringSingleton` hasta que exista un proveedor común (por ejemplo en `ServiceCommon`):
  ```csharp
  builder.Services.AddMembershipModule(_ => ConnectionStringSingleton.Instance.PostgresConnection);
  ```
- Las Razor Pages (`Pages/Memberships/*.cshtml.cs`, `Pages/DetailsUsers/DetailsUsers.cshtml.cs`) ahora consumen los contratos expuestos por `ServiceMembership.Application.Interfaces` y sus entidades de dominio.
- `SelectDataService` obtiene las opciones de membresía a través del servicio del módulo.
- El módulo expone también `IDetailUserService`, usado por la pantalla de suscripciones (`DetailsUsers`).

## Cambios en el proyecto web
- Se eliminaron las entidades, repositorios y servicios de membresías y detalles de usuario en `GYMPT/Domain`, `GYMPT/Application` y `GYMPT/Infrastructure`.
- `RepositoryFactory` ya no crea repositorios para `Membership` ni `DetailsUser`.
- Las páginas y servicios solo dependen de los contratos del módulo.

## Próximos pasos sugeridos
1. Ejecutar `dotnet run` y verificar manualmente la funcionalidad de las páginas de membresías y detalles de usuario.
2. Cuando exista `ServiceCommon`, mover `ConnectionStringSingleton` o implementar un `IMembershipConnectionProvider` compartido para que todos los módulos reutilicen la configuración de base de datos.
