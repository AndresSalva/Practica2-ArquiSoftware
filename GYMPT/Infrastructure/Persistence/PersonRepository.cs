using Dapper;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Services;
using Npgsql;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Infrastructure.Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _postgresString;

        public PersonRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Person> CreateAsync(Person entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creating new person: {entity.Name} {entity.FirstLastname}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    INSERT INTO person 
                        (name, first_lastname, second_lastname, date_birth, ci, created_at, last_modification, is_active)
                    VALUES 
                        (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @CreatedAt, @LastModification, @IsActive)
                    RETURNING id;";

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to create person '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Deactivating person: {id}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    UPDATE person 
                    SET is_active = false, 
                        last_modification = @LastModification 
                    WHERE id = @Id;";

                var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to deactivate person (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Retrieving all active persons.");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    SELECT 
                        id, 
                        name, 
                        first_lastname AS FirstLastname, 
                        second_lastname AS SecondLastname, 
                        date_birth AS DateBirth, 
                        ci, 
                        created_at AS CreatedAt, 
                        last_modification AS LastModification, 
                        is_active AS IsActive
                    FROM person
                    WHERE is_active = true;";

                return await conn.QueryAsync<Person>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to get all persons: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Retrieving person by id: {id}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    SELECT 
                        id, 
                        name, 
                        first_lastname AS FirstLastname, 
                        second_lastname AS SecondLastname, 
                        date_birth AS DateBirth, 
                        ci, 
                        created_at AS CreatedAt, 
                        last_modification AS LastModification, 
                        is_active AS IsActive
                    FROM person
                    WHERE id = @Id AND is_active = true;";

                return await conn.QuerySingleOrDefaultAsync<Person>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to get person by Id ({id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<Person> UpdateAsync(Person entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Updating person: {entity.Id}");
                using var conn = new NpgsqlConnection(_postgresString);

                const string sql = @"
                    UPDATE person 
                    SET 
                        name = @Name,
                        first_lastname = @FirstLastname,
                        second_lastname = @SecondLastname,
                        date_birth = @DateBirth,
                        ci = @Ci,
                        last_modification = @LastModification,
                        is_active = @IsActive
                    WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to update person (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }
    }
}
