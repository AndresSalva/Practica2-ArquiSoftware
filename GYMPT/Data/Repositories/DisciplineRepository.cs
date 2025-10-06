using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class DisciplineRepository : IRepository<Discipline>
    {
        private readonly string _postgresString;

        public DisciplineRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Discipline> CreateAsync(Discipline entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creando nueva disciplina: {entity.Name}.");
            var sql = @"
                INSERT INTO discipline 
                (name, id_instructor, start_time, end_time, created_at, last_modification, ""isActive"")
                VALUES (@Name, @IdInstructor, @StartTime, @EndTime, @CreatedAt, @LastModification, @IsActive)
                RETURNING id;";

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

        public async Task<Discipline> GetByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Buscando disciplina con ID: {id}.");
            var sql = @"SELECT id, 
                               name, 
                               id_instructor AS IdInstructor, 
                               start_time AS StartTime, 
                               end_time AS EndTime, 
                               created_at AS CreatedAt, 
                               last_modification AS LastModification, 
                               is_active as IsActive 
                        FROM discipline 
                        WHERE id = @Id;";

            using (var conn = new NpgsqlConnection(_postgresString))
            {
                return await conn.QueryFirstOrDefaultAsync<Discipline>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de disciplinas con Dapper.");
            var sql = @"SELECT id, 
                               name, 
                               id_instructor AS IdInstructor, 
                               start_time AS StartTime, 
                               end_time AS EndTime, 
                               created_at AS CreatedAt, 
                               last_modification AS LastModification, 
                               is_active as IsActive 
                        FROM discipline;";

            using (var conn = new NpgsqlConnection(_postgresString))
            {
                return await conn.QueryAsync<Discipline>(sql);
            }
        }

        public async Task<Discipline> UpdateAsync(Discipline entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando disciplina con ID: {entity.Id}.");
            var sql = @"UPDATE discipline 
                        SET name = @Name,
                            id_instructor = @IdInstructor,
                            start_time = @StartTime,
                            end_time = @EndTime,
                            last_modification = @LastModification,
                            is_active = @IsActive
                        WHERE id = @Id;";

            using (var conn = new NpgsqlConnection(_postgresString))
            {
                entity.LastModification = DateTime.UtcNow;

                var affectedRows = await conn.ExecuteAsync(sql, entity);
                if (affectedRows == 0)
                {
                    throw new KeyNotFoundException("No se encontr� una disciplina con el ID proporcionado para actualizar.");
                }
            }
            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Dando de baja disciplina con ID: {id}.");
            var sql = @"UPDATE discipline 
                SET is_active = false, last_modification = @LastModification 
                WHERE id = @Id;";

            using (var conn = new NpgsqlConnection(_postgresString))
            {
                var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affectedRows > 0;
            }
        }
    }
}
