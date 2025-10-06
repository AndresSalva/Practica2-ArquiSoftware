using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly string _postgresString;

        public UserRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de usuarios con Dapper.");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql =
                @"SELECT id AS Id,
                created_at AS CreatedAt,
                last_modification AS LastModification,
                is_active AS IsActive,
                name AS Name,
                first_lastname AS FirstLastname,
                second_lastname AS SecondLastname,
                date_birth AS DateBirth,
                ci AS Ci,
                ""role"" AS Role
                FROM ""user""
                WHERE is_active = true;";

                return await conn.QueryAsync<User>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener todos los usuarios: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Solicitando usuario con ID: {id} con Dapper.");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql =
                @"SELECT id AS Id,
                name AS Name,
                first_lastname AS FirstLastname,
                second_lastname AS SecondLastname,
                date_birth AS DateBirth,
                ci AS Ci,
                ""role"" AS Role,
                created_at AS CreatedAt,
                last_modification AS LastModification,
                is_active as IsActive
                FROM ""user""
                WHERE id = @Id AND is_active = true;";
                var resp = await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
                return resp;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener el usuario (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<User> CreateAsync(User entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creando un nuevo usuario: {entity.Name} {entity.FirstLastname}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql =
                @"INSERT INTO ""user""
                (name, first_lastname, second_lastname, date_birth, ci, ""role"", created_at, last_modification, is_active)
                VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive)
                RETURNING id;";

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al crear el usuario '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando usuario con Id: {entity.Id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql =
                @"UPDATE ""user""
                SET name = @Name,
                first_lastname = @FirstLastname,
                second_lastname = @SecondLastname,
                date_birth = @DateBirth,
                ci = @CI,
                ""role"" = @Role,
                last_modification = @LastModification,
                is_active = @IsActive
                WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar el usuario (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminando usuario con Id: {id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"UPDATE ""user"" SET is_active = false WHERE id = @Id;";
                var affected = await conn.ExecuteAsync(sql, new { Id = id });
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al eliminar el usuario (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }
    }
}
