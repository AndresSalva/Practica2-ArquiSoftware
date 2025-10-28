using Dapper;
using Microsoft.Extensions.Logging;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using System.Data;

namespace ServiceClient.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly IClientConnectionProvider _connectionProvider;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(IClientConnectionProvider connectionProvider, ILogger<UserRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));

            _connectionProvider = connectionProvider;
            _logger = logger;
        }

        private IDbConnection CreateConnection() => _connectionProvider.CreateConnection();

        public async Task<User> CreateAsync(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _logger.LogInformation("Creando nuevo usuario: {UserName} {UserLastName}", entity.Name, entity.FirstLastname);

            const string sql = """
                INSERT INTO "user" (name, first_lastname, second_lastname, date_birth, ci, "role", created_at, last_modification, is_active)
                VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive)
                RETURNING id;
                """;

            // Cambiado de 'await using' a 'using'
            using var conn = CreateConnection();

            entity.CreatedAt = DateTime.UtcNow;
            entity.LastModification = DateTime.UtcNow;
            entity.IsActive = true;

            var newId = await conn.ExecuteScalarAsync<int>(sql, entity);
            entity.Id = newId;

            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            _logger.LogInformation("Realizando borrado lógico del usuario con Id: {UserId}", id);

            const string sql = """
                UPDATE "user"
                SET is_active = false,
                    last_modification = @LastModification
                WHERE id = @Id;
                """;

            // Cambiado de 'await using' a 'using'
            using var conn = CreateConnection();

            var affectedRows = await conn.ExecuteAsync(sql, new
            {
                Id = id,
                LastModification = DateTime.UtcNow
            });

            return affectedRows > 0;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo la lista de usuarios activos");

            const string sql = """
                SELECT id,
                       name,
                       first_lastname AS FirstLastname,
                       second_lastname AS SecondLastname,
                       date_birth AS DateBirth,
                       ci,
                       "role" AS Role,
                       created_at AS CreatedAt,
                       last_modification AS LastModification,
                       is_active AS IsActive
                FROM "user"
                WHERE is_active = true;
                """;

            // Cambiado de 'await using' a 'using'
            using var conn = CreateConnection();
            return await conn.QueryAsync<User>(sql);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo usuario con Id: {UserId}", id);

            const string sql = """
                SELECT id,
                       name,
                       first_lastname AS FirstLastname,
                       second_lastname AS SecondLastname,
                       date_birth AS DateBirth,
                       ci,
                       "role" AS Role,
                       created_at AS CreatedAt,
                       last_modification AS LastModification,
                       is_active as IsActive
                FROM "user"
                WHERE id = @Id AND is_active = true;
                """;

            // Cambiado de 'await using' a 'using'
            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> UpdateAsync(User entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));
            _logger.LogInformation("Actualizando usuario con Id: {UserId}", entity.Id);

            const string sql = """
                UPDATE "user"
                SET name = @Name,
                    first_lastname = @FirstLastname,
                    second_lastname = @SecondLastname,
                    date_birth = @DateBirth,
                    ci = @CI,
                    "role" = @Role,
                    last_modification = @LastModification,
                    is_active = @IsActive
                WHERE id = @Id;
                """;

            // Cambiado de 'await using' a 'using'
            using var conn = CreateConnection();

            entity.LastModification = DateTime.UtcNow;

            var affectedRows = await conn.ExecuteAsync(sql, entity);

            return affectedRows > 0 ? entity : null;
        }
    }
}