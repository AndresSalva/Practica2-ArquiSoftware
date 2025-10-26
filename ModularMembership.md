# Servicio de Membresías (Modular)

## Objetivo
Separar toda la lógica de membresías en un módulo reutilizable (`ServiceMembership`) que pueda integrarse en otros servicios (por ejemplo, `ServiceCommon`) sin depender de implementaciones internas del proyecto web.

## Estructura del nuevo módulo
```
ServiceMembership/
 ├── Application/
 │   ├── Interfaces/IMembershipService.cs
 │   └── Services/MembershipService.cs
 ├── Domain/
 │   ├── Entities/Membership.cs
 │   └── Ports/IMembershipRepository.cs
 ├── Infrastructure/
 │   ├── DependencyInjection/MembershipModuleServiceCollectionExtensions.cs
 │   ├── Persistence/MembershipRepository.cs
 │   └── Providers/IMembershipConnectionProvider.cs
 └── ServiceMembership.csproj
```

### Capas principales
- **Domain**: entidad `Membership` y contrato `IMembershipRepository` independientes del proyecto web.
- **Application**: servicio `IMembershipService` que implementa la lógica de negocio y usa el repositorio.
- **Infrastructure**: repositorio Dapper (`MembershipRepository`), proveedor de cadena de conexión y extensión de DI `AddMembershipModule`.

## Integración con el proyecto web
- `GYMPT/GYMPT.csproj` referencia el nuevo proyecto (`ServiceMembership.csproj`).
- `Program.cs` registra el módulo con la cadena de conexión existente:  
  ```csharp
  builder.Services.AddMembershipModule(_ => ConnectionStringSingleton.Instance.PostgresConnection);
  ```
  De esta forma seguimos reutilizando `ConnectionStringSingleton` hasta que exista un `ServiceCommon`.
- Las páginas de Razor (`Pages/Memberships/*.cshtml.cs` y `Pages/DetailsUsers/DetailsUsers.cshtml.cs`) ahora consumen `ServiceMembership.Application.Interfaces.IMembershipService` y `ServiceMembership.Domain.Entities.Membership`.
- `SelectDataService` usa el nuevo servicio para entregar los combos de membresías en la UI.

## Qué se removió del proyecto web
- Entidad, repositorio, servicio y creadores de membresías que vivían en `Domain`, `Application` e `Infrastructure` de `GYMPT`.
- Dependencia de `RepositoryFactory` hacia `Membership`.

## Próximos pasos sugeridos
1. Ejecutar `dotnet run` y probar manualmente las páginas de membresías y la pantalla de detalles de usuario.
2. Cuando exista `ServiceCommon`, implementar un `IMembershipConnectionProvider` común o mover `ConnectionStringSingleton` allí para compartirlo con otros módulos.
