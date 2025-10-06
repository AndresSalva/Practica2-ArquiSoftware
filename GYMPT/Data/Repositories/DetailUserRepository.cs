using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly string _connectionString;

        public DetailUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Obteniendo todos los detalles de usuarios.");
                using var conn = new NpgsqlConnection(_connectionString);

                var sql = @"
                    SELECT 
                        id, 
                        id_user AS IdUser, 
                        id_membership AS IdMembership,
                        start_date AS StartDate, 
                        end_date AS EndDate,
                        sessions_left AS SessionsLeft, 
                        created_at AS CreatedAt,
                        last_modification AS LastModification, 
                        ""isActive"" AS IsActive
                    FROM ""Details_user"";";

                var result = await conn.QueryAsync<DetailsUser>(sql);
                await RemoteLoggerSingleton.Instance.LogInfo($"Se encontraron {result?.Count() ?? 0} detalles de usuario.");
                return result;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error en GetAllAsync: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<DetailsUser> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Obteniendo detalle de usuario con ID: {id}");
                using var conn = new NpgsqlConnection(_connectionString);

                var sql = @"
                    SELECT 
                        id, 
                        id_user AS IdUser, 
                        id_membership AS IdMembership,
                        start_date AS StartDate, 
                        end_date AS EndDate,
                        sessions_left AS SessionsLeft, 
                        created_at AS CreatedAt,
                        last_modification AS LastModification, 
                        ""isActive"" AS IsActive
                    FROM ""Details_user""
                    WHERE id = @Id;";

                return await conn.QueryFirstOrDefaultAsync<DetailsUser>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error en GetByIdAsync: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<DetailsUser> CreateAsync(DetailsUser entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creando detalle de usuario: UserId={entity.IdUser}, MembershipId={entity.IdMembership}");

                using var conn = new NpgsqlConnection(_connectionString);

                // Validar que el usuario existe
                var userExists = await conn.ExecuteScalarAsync<bool>(
                    @"SELECT COUNT(1) FROM ""User"" WHERE id = @UserId AND ""isActive"" = true",
                    new { UserId = entity.IdUser });

                if (!userExists)
                {
                    throw new ArgumentException($"El usuario con ID {entity.IdUser} no existe o no est� activo.");
                }

                // Validar que la membres�a existe
                var membershipExists = await conn.ExecuteScalarAsync<bool>(
                    @"SELECT COUNT(1) FROM ""Membership"" WHERE id = @MembershipId AND ""isActive"" = true",
                    new { MembershipId = entity.IdMembership });

                if (!membershipExists)
                {
                    throw new ArgumentException($"La membres�a con ID {entity.IdMembership} no existe o no est� activa.");
                }

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;
                entity.SessionsLeft = entity.SessionsLeft < 0 ? 0 : entity.SessionsLeft;

                var sql = @"
                    INSERT INTO ""Details_user"" 
                    (id_user, id_membership, start_date, end_date, sessions_left, created_at, last_modification, ""isActive"")
                    VALUES (@IdUser, @IdMembership, @StartDate, @EndDate, @SessionsLeft, @CreatedAt, @LastModification, @IsActive)
                    RETURNING id;";

                entity.Id = await conn.QuerySingleAsync<int>(sql, entity);

                await RemoteLoggerSingleton.Instance.LogInfo($"Detalle de usuario creado exitosamente con ID: {entity.Id}");

                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error en CreateAsync: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<DetailsUser> UpdateAsync(DetailsUser entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando detalle de usuario con ID: {entity.Id}");
                using var conn = new NpgsqlConnection(_connectionString);

                entity.LastModification = DateTime.UtcNow;
                entity.SessionsLeft = entity.SessionsLeft < 0 ? 0 : entity.SessionsLeft;

                var sql = @"
                    UPDATE ""Details_user"" 
                    SET id_user = @IdUser,
                        id_membership = @IdMembership,
                        start_date = @StartDate,
                        end_date = @EndDate,
                        sessions_left = @SessionsLeft,
                        last_modification = @LastModification,
                        ""isActive"" = @IsActive
                    WHERE id = @Id;";

                var affectedRows = await conn.ExecuteAsync(sql, entity);

                if (affectedRows == 0)
                    throw new KeyNotFoundException("No se encontr� el detalle de usuario para actualizar.");

                await RemoteLoggerSingleton.Instance.LogInfo($"Detalle de usuario actualizado exitosamente: {entity.Id}");
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error en UpdateAsync: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminando detalle de usuario con ID: {id}");
                using var conn = new NpgsqlConnection(_connectionString);

                var sql = @"DELETE FROM ""Details_user"" WHERE id = @Id;";
                var affected = await conn.ExecuteAsync(sql, new { Id = id });

                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminaci�n completada. Filas afectadas: {affected}");
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error en DeleteByIdAsync: {ex.Message}", ex);
                throw;
            }
        }
    }
}