using Dapper;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Services;
using Npgsql;

namespace GYMPT.Infrastructure.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly string _postgresString;

        public UserRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<User> CreateAsync(User entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creating new user: {entity.Name} {entity.FirstLastname}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"INSERT INTO ""user"" (name, first_lastname, second_lastname, date_birth, ci, ""role"", created_at, last_modification, is_active) VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive) RETURNING id;";

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to create user '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Deleting user: {id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"UPDATE ""user"" SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
                var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to eliminate user (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Trying to obtain full list");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"SELECT id, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth AS DateBirth, ci, ""role"" AS Role FROM ""user"" WHERE is_active = true;";
                return await conn.QueryAsync<User>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to obtain all users: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Trying to obtain user with id: {id} con Dapper.");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"SELECT id, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth AS DateBirth, ci, ""role"" AS Role, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM ""user"" WHERE id = @Id AND is_active = true;";
                return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to obtain user with (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<User> UpdateAsync(User entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Updating user with id: {entity.Id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"UPDATE ""user"" SET name = @Name, first_lastname = @FirstLastname, second_lastname = @SecondLastname, date_birth = @DateBirth, ci = @CI, ""role"" = @Role, last_modification = @LastModification, is_active = @IsActive WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to update user (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }
    }
}