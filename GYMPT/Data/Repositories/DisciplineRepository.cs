using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services; // Asegúrate de que esto incluye RemoteLoggerSingleton
using Microsoft.Extensions.Configuration;
using Npgsql;
using System; // Necesario para Exception
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class DisciplineRepository : IRepository<Discipline> // Esta línea indica que debe implementar todos los métodos de IRepository<Discipline>
    {
        private readonly string _connectionString;

        public DisciplineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de disciplinas con Dapper.");
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    // Ajusta los alias si tus nombres de columna en la BD son diferentes de snake_case
                    var sql = "SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, \"isActive\" as IsActive FROM \"Discipline\"";
                    return await conn.QueryAsync<Discipline>(sql);
                }
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener todas las disciplinas: {ex.Message}", ex);
                throw; // Re-lanza la excepción o maneja de otra forma
            }
        }

        // --- IMPLEMENTACIÓN DE LOS MÉTODOS FALTANTES PARA CUMPLIR CON IRepository<Discipline> ---

        // Agrega este método para implementar IRepository<Discipline>.GetByIdAsync(int)
        public async Task<Discipline> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Solicitando disciplina con ID: {id} con Dapper.");
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    var sql = @"SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime,
                                created_at AS CreatedAt, last_modification AS LastModification, ""isActive"" as IsActive
                                FROM ""Discipline""
                                WHERE id = @Id;";
                    return await conn.QuerySingleOrDefaultAsync<Discipline>(sql, new { Id = id });
                }
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener la disciplina (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Discipline> CreateAsync(Discipline entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creando una nueva disciplina: {entity.Name}");
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    var sql = @"INSERT INTO ""Discipline""
                                (name, id_instructor, start_time, end_time, created_at, ""isActive"")
                                VALUES (@Name, @IdInstructor, @StartTime, @EndTime, NOW(), @IsActive)
                                RETURNING id;"; // Asume que 'id' es SERIAL/BIGSERIAL y la BD lo genera
                    entity.Id = await conn.ExecuteScalarAsync<long>(sql, entity); // Asumo que Id es de tipo long
                    return entity;
                }
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al crear la disciplina '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Discipline> UpdateAsync(Discipline entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando disciplina con Id: {entity.Id}");
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    var sql = @"UPDATE ""Discipline""
                                SET name = @Name,
                                    id_instructor = @IdInstructor,
                                    start_time = @StartTime,
                                    end_time = @EndTime,
                                    last_modification = NOW(),
                                    ""isActive"" = @IsActive
                                WHERE id = @Id;";
                    await conn.ExecuteAsync(sql, entity);
                    return entity;
                }
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar la disciplina (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminando disciplina con Id: {id}");
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    var sql = @"DELETE FROM ""Discipline"" WHERE id = @Id;";
                    var affectedRows = await conn.ExecuteAsync(sql, new { Id = id });
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al eliminar la disciplina (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }
    }
}