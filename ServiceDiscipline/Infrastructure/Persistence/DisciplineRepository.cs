using Dapper;
using Npgsql;
using ServiceCommon.Domain.Ports;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;

namespace ServiceDiscipline.Infrastructure.Persistence
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly string _connectionString;
        private readonly IRemoteLogger _remoteLogger;
        public DisciplineRepository(IRemoteLogger remoteLogger, IConnectionStringProvider connectionStringProvider)
        {
            _connectionString = connectionStringProvider.GetPostgresConnection();
            _remoteLogger = remoteLogger;
        }

        public async Task<Discipline> CreateAsync(Discipline entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            //await _logger.LogInfo($"Creating new discipline: {entity.Name}.");
            const string sql = """
                INSERT INTO discipline (name, id_instructor, start_time, end_time, created_at, last_modification, is_active) 
                VALUES (@Name, @IdInstructor, @StartTime, @EndTime, @CreatedAt, @LastModification, @IsActive) 
                RETURNING id;
                """;

            await using var conn = new NpgsqlConnection(_connectionString);

            entity.CreatedAt = DateTime.UtcNow;
            entity.LastModification = DateTime.UtcNow;
            entity.IsActive = true;

            var newId = await conn.QuerySingleAsync<short>(sql, entity);
            entity.Id = newId;

            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            //await _logger.LogInfo($"Deleting discipline with id: {id}.");

            const string sql = """
                UPDATE discipline 
                SET is_active = false, last_modification = @LastModification 
                WHERE id = @Id;
                """;
            await using var conn = new NpgsqlConnection(_connectionString);
            var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
            return affectedRows > 0;
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            //await _logger.LogInfo($"Fetching active discipline list");
            const string sql = """
                SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive 
                FROM discipline 
                WHERE is_active = true
                ORDER BY name ASC;
                """;

            await using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Discipline>(sql);
        }

        public async Task<Discipline> GetByIdAsync(int id)
        {
            //await _logger.LogInfo($"Fetching discipline with id: {id}.");

            const string sql = """
                SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, 
                created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive 
                FROM discipline WHERE id = @Id;
                """;
            await using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QuerySingleOrDefaultAsync<Discipline>(sql, new { Id = id });
        }

        public async Task<Discipline> UpdateAsync(Discipline entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            //await _logger.LogInfo($"Deleting discipline with id: {entity.Id}.");

            const string sql = """
                UPDATE discipline SET name = @Name, id_instructor = @IdInstructor, start_time = @StartTime, 
                end_time = @EndTime, last_modification = @LastModification, is_active = @IsActive 
                WHERE id = @Id;
                """;
            await using var conn = new NpgsqlConnection(_connectionString);

            entity.LastModification = DateTime.UtcNow;

            var affectedRows = await conn.ExecuteAsync(sql, entity);

            return affectedRows > 0 ? entity : null;
        }
    }
}