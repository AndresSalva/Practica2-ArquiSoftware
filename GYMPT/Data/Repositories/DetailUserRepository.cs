using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly string _connectionString;

        public DetailUserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // CREATE
        public async Task<DetailsUser?> CreateAsync(DetailsUser entity)
        {
            try
            {
                RemoteLoggerSingleton.Instance.LogInfo("Creando un nuevo detalle de usuario...");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = @"INSERT INTO details_user 
                            (created_at, last_modification, isactive, id_user, id_membership, start_date, end_date, sessions_left) 
                            VALUES (@created_at, @last_modification, @isactive, @id_user, @id_membership, @start_date, @end_date, @sessions_left) 
                            RETURNING id;";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("created_at", entity.CreatedAt);
                cmd.Parameters.AddWithValue("last_modification", (object?)entity.LastModification ?? DBNull.Value);
                cmd.Parameters.AddWithValue("isactive", entity.IsActive ?? true);
                cmd.Parameters.AddWithValue("id_user", entity.IdUser);
                cmd.Parameters.AddWithValue("id_membership", entity.IdMembership);
                cmd.Parameters.AddWithValue("start_date", entity.StartDate);
                cmd.Parameters.AddWithValue("end_date", entity.EndDate);
                cmd.Parameters.AddWithValue("sessions_left", (object?)entity.SessionsLeft ?? DBNull.Value);

                var id = (long)await cmd.ExecuteScalarAsync();
                entity.Id = id;

                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Error al crear detalle de usuario.", ex);
                throw;
            }
        }

        // READ ALL
        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            var results = new List<DetailsUser>();

            try
            {
                RemoteLoggerSingleton.Instance.LogInfo("Obteniendo todos los detalles de usuarios...");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = "SELECT * FROM details_user WHERE isactive = true;";
                using var cmd = new NpgsqlCommand(sql, conn);
                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    results.Add(new DetailsUser
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        LastModification = reader.IsDBNull(reader.GetOrdinal("last_modification")) ? null : reader.GetDateTime(reader.GetOrdinal("last_modification")),
                        IsActive = reader.IsDBNull(reader.GetOrdinal("isactive")) ? null : reader.GetBoolean(reader.GetOrdinal("isactive")),
                        IdUser = reader.GetInt64(reader.GetOrdinal("id_user")),
                        IdMembership = reader.GetInt64(reader.GetOrdinal("id_membership")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                        SessionsLeft = reader.IsDBNull(reader.GetOrdinal("sessions_left")) ? null : reader.GetInt32(reader.GetOrdinal("sessions_left"))
                    });
                }

                return results;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Error al obtener lista de detalles de usuario.", ex);
                throw;
            }
        }

        // READ BY ID
        public async Task<DetailsUser?> GetByIdAsync(long id)
        {
            try
            {
                RemoteLoggerSingleton.Instance.LogInfo($"Buscando detalle de usuario con ID {id}...");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = "SELECT * FROM details_user WHERE id = @id;";
                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("id", id);

                using var reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new DetailsUser
                    {
                        Id = reader.GetInt64(reader.GetOrdinal("id")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at")),
                        LastModification = reader.IsDBNull(reader.GetOrdinal("last_modification")) ? null : reader.GetDateTime(reader.GetOrdinal("last_modification")),
                        IsActive = reader.IsDBNull(reader.GetOrdinal("isactive")) ? null : reader.GetBoolean(reader.GetOrdinal("isactive")),
                        IdUser = reader.GetInt64(reader.GetOrdinal("id_user")),
                        IdMembership = reader.GetInt64(reader.GetOrdinal("id_membership")),
                        StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                        EndDate = reader.GetDateTime(reader.GetOrdinal("end_date")),
                        SessionsLeft = reader.IsDBNull(reader.GetOrdinal("sessions_left")) ? null : reader.GetInt32(reader.GetOrdinal("sessions_left"))
                    };
                }

                return null;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al buscar detalle de usuario con ID {id}.", ex);
                throw;
            }
        }

        // UPDATE
        public async Task<DetailsUser?> UpdateAsync(DetailsUser entity)
        {
            try
            {
                RemoteLoggerSingleton.Instance.LogInfo($"Actualizando detalle de usuario con ID {entity.Id}...");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = @"UPDATE details_user 
                            SET last_modification = @last_modification, 
                                isactive = @isactive, 
                                id_user = @id_user, 
                                id_membership = @id_membership, 
                                start_date = @start_date, 
                                end_date = @end_date, 
                                sessions_left = @sessions_left 
                            WHERE id = @id;";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("last_modification", (object?)entity.LastModification ?? DBNull.Value);
                cmd.Parameters.AddWithValue("isactive", entity.IsActive ?? true);
                cmd.Parameters.AddWithValue("id_user", entity.IdUser);
                cmd.Parameters.AddWithValue("id_membership", entity.IdMembership);
                cmd.Parameters.AddWithValue("start_date", entity.StartDate);
                cmd.Parameters.AddWithValue("end_date", entity.EndDate);
                cmd.Parameters.AddWithValue("sessions_left", (object?)entity.SessionsLeft ?? DBNull.Value);
                cmd.Parameters.AddWithValue("id", entity.Id);

                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar detalle de usuario con ID {entity.Id}.", ex);
                throw;
            }
        }

        // DELETE (LÓGICO)
        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                RemoteLoggerSingleton.Instance.LogInfo($"Marcando como inactivo el detalle de usuario con ID {id}...");

                using var conn = new NpgsqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = @"UPDATE details_user 
                            SET isactive = false, last_modification = @last_modification 
                            WHERE id = @id;";

                using var cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("last_modification", DateTime.UtcNow);
                cmd.Parameters.AddWithValue("id", id);

                var rows = await cmd.ExecuteNonQueryAsync();
                return rows > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al eliminar detalle de usuario con ID {id}.", ex);
                throw;
            }
        }
    }
}
