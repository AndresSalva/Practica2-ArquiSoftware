using Dapper;
using Npgsql;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Infrastructure.Persistence
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly string _postgresString;

        public MembershipRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Membership> CreateAsync(Membership entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creating new membership: {entity.Name}");

            var sql = @"
        INSERT INTO membership 
        (name, price, description, monthly_sessions, created_at, last_modification, is_active) 
        VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @LastModification, @IsActive) 
        RETURNING id;
    ";

            using (var conn = new NpgsqlConnection(_postgresString))
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                // Convierte explícitamente de int a short (por SMALLSERIAL)
                entity.Id = (short)await conn.ExecuteScalarAsync<int>(sql, entity);
            }

            return entity;
        }


        public async Task<Membership> UpdateAsync(Membership entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Updating membership: {entity.Name}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    UPDATE membership 
                    SET 
                        name = @Name, 
                        price = @Price, 
                        description = @Description, 
                        monthly_sessions = @MonthlySessions, 
                        last_modification = @LastModification, 
                        is_active = @IsActive
                    WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                var affected = await conn.ExecuteAsync(sql, entity);

                if (affected == 0)
                    throw new KeyNotFoundException($"No membership found with id {entity.Id} to update.");

                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error updating membership '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Deactivating membership: {id}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    UPDATE membership 
                    SET is_active = false, 
                        last_modification = @LastModification 
                    WHERE id = @Id;";

                var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error deleting membership (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Membership?> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Retrieving membership by id: {id}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    SELECT 
                        id, 
                        name, 
                        price, 
                        description, 
                        monthly_sessions AS MonthlySessions, 
                        created_at AS CreatedAt, 
                        last_modification AS LastModification, 
                        is_active AS IsActive
                    FROM membership
                    WHERE id = @Id;";

                return await conn.QuerySingleOrDefaultAsync<Membership>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error retrieving membership (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Retrieving all active memberships.");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    SELECT 
                        id, 
                        name, 
                        price, 
                        description, 
                        monthly_sessions AS MonthlySessions, 
                        created_at AS CreatedAt, 
                        last_modification AS LastModification, 
                        is_active AS IsActive
                    FROM membership
                    WHERE is_active = true;";

                return await conn.QueryAsync<Membership>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error retrieving memberships: {ex.Message}", ex);
                throw;
            }
        }
    }
}
