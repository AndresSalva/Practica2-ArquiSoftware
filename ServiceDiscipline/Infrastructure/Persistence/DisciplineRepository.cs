using Dapper;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;
using Npgsql;

namespace ServiceDiscipline.Infrastructure.Persistence
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly string _postgresString;

        public DisciplineRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Discipline> CreateAsync(Discipline entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creating new discipline: {entity.Name}.");
            var sql = @"INSERT INTO discipline (name, id_instructor, start_time, end_time, created_at, last_modification, is_active) VALUES (@Name, @IdInstructor, @StartTime, @EndTime, @CreatedAt, @LastModification, @IsActive) RETURNING id;";

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
            await RemoteLoggerSingleton.Instance.LogInfo($"Deleting discipline with id: {id}.");
            var sql = @"UPDATE discipline SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Searching for discipline list.");
            var sql = @"SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM discipline WHERE is_active = true;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                return await conn.QueryAsync<Discipline>(sql);
            }
        }

        public async Task<Discipline> GetByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Searching discipline with: {id}.");
            var sql = @"SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM discipline WHERE id = @Id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                return await conn.QueryFirstOrDefaultAsync<Discipline>(sql, new { Id = id });
            }
        }

        public async Task<Discipline> UpdateAsync(Discipline entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Updating discipline with: {entity.Id}.");
            var sql = @"UPDATE discipline SET name = @Name, id_instructor = @IdInstructor, start_time = @StartTime, end_time = @EndTime, last_modification = @LastModification, is_active = @IsActive WHERE id = @Id;";
            using (var conn = new NpgsqlConnection(_postgresString))
            {
                entity.LastModification = DateTime.UtcNow;
                var affectedRows = await conn.ExecuteAsync(sql, entity);
                if (affectedRows == 0)
                {
                    throw new KeyNotFoundException("ID disciplne wasn't found for update.");
                }
            }
            return entity;
        }
    }
}