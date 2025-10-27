using Dapper;
using Microsoft.Extensions.Logging;
using ServiceDiscipline.Domain.Entities;
using ServiceDiscipline.Domain.Ports;
using ServiceDiscipline.Infrastructure.Provider;
using Npgsql;

namespace ServiceDiscipline.Infrastructure.Persistence
{
    public class DisciplineRepository : IDisciplineRepository
    {
        private readonly string _connectionString;
        private readonly ILogger<DisciplineRepository> _logger;
        public DisciplineRepository(IDisciplineConnectionProvider connectionProvider, ILogger<DisciplineRepository> logger)
        {
            ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
            ArgumentNullException.ThrowIfNull(logger, nameof(logger));
            var connectionString = connectionProvider.GetConnectionString();
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("El service de Disciplinas requiere una cadena de conexión válida.");
            }

            _connectionString = connectionString;
            _logger = logger;
        }

        public async Task<Discipline> CreateAsync(Discipline entity)
        {
            ArgumentNullException.ThrowIfNull(entity, nameof(entity));

            _logger.LogInformation("Creating discipline {DisciplineName}", entity.Name);
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
            _logger.LogInformation("Soft deleting discipline {DisciplineId}", id);

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
            _logger.LogInformation("Fetching active discipline list");

            const string sql = """
                
                SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive 
                FROM discipline 
                WHERE is_active = true;
                
                """;

            await using var conn = new NpgsqlConnection(_connectionString);
            return await conn.QueryAsync<Discipline>(sql);
        }

        public async Task<Discipline> GetByIdAsync(int id)
        {
            _logger.LogInformation("Fetching discipline {DisciplineId}", id);

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

            _logger.LogInformation("Updating discipline {DisciplineId}", entity.Id);

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