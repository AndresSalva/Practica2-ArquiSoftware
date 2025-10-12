using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class MembershipRepository : IRepository<Membership>
    {
        private readonly string _postgresString;

        public MembershipRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Membership> CreateAsync(Membership entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creating new membership: {entity.Name}");
            var sql = @"INSERT INTO membership (name, price, description, monthly_sessions, created_at, last_modification, is_active) VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @LastModification, @IsActive) RETURNING id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;
                var newId = await conn.QuerySingleAsync<short>(sql, entity);
                entity.Id = newId;
            }
            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Deleting membership with id: {id}.");
            var sql = @"UPDATE membership SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Searching for membership list");
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                var sql = @"SELECT id, name, price, description, monthly_sessions AS MonthlySessions, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM membership WHERE is_active = true;";
                return await conn.QueryAsync<Membership>(sql);
            }
        }

        public async Task<Membership> GetByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Searching for membership list with id: {id}");
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                var sql = @"SELECT id, name, price, description, monthly_sessions AS MonthlySessions, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM membership WHERE id = @Id;";
                return await conn.QuerySingleOrDefaultAsync<Membership>(sql, new { Id = id });
            }
        }

        public async Task<Membership> UpdateAsync(Membership entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Updating membershi[: {entity.Name}");
            var sql = @"UPDATE membership SET name = @Name, price = @Price, description = @Description, monthly_sessions = @MonthlySessions, last_modification = @LastModification, is_active = @IsActive WHERE id = @Id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                entity.LastModification = DateTime.UtcNow;
                var affectedRows = await conn.ExecuteAsync(sql, entity);
                if (affectedRows == 0)
                {
                    throw new KeyNotFoundException("We doesn't found a membership to update.");
                }
            }
            return entity;
        }
    }
}