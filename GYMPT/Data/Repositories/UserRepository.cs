using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class UserRepository : IRepository<UserData>
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<UserData>> GetAllAsync()
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de usuarios con Dapper.");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"SELECT id, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                                   date_birth AS DateBirth, ci AS CI, role AS Role,
                                   created_at AS CreatedAt, last_modification AS LastModification, ""isActive"" as IsActive
                            FROM ""Users""";
                return await conn.QueryAsync<UserData>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener todos los usuarios: {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserData> GetByIdAsync(long id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Solicitando usuario con ID: {id} con Dapper.");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"SELECT id, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                                   date_birth AS DateBirth, ci AS CI, role AS Role,
                                   created_at AS CreatedAt, last_modification AS LastModification, ""isActive"" as IsActive
                            FROM ""Users""
                            WHERE id = @Id;";
                return await conn.QuerySingleOrDefaultAsync<UserData>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener el usuario (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserData> CreateAsync(UserData entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Creando un nuevo usuario: {entity.Name} {entity.FirstLastname}");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"INSERT INTO ""Users""
                               (name, first_lastname, second_lastname, date_birth, ci, role, created_at, last_modification, ""isActive"")
                            VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @CI, @Role, @CreatedAt, @LastModification, @IsActive)
                            RETURNING id;";

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                entity.Id = await conn.ExecuteScalarAsync<long>(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al crear el usuario '{entity.Name}': {ex.Message}", ex);
                throw;
            }
        }

        public async Task<UserData> UpdateAsync(UserData entity)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando usuario con Id: {entity.Id}");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"UPDATE ""Users""
                            SET name = @Name,
                                first_lastname = @FirstLastname,
                                second_lastname = @SecondLastname,
                                date_birth = @DateBirth,
                                ci = @CI,
                                role = @Role,
                                last_modification = @LastModification,
                                ""isActive"" = @IsActive
                            WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar el usuario (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(long id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminando usuario con Id: {id}");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"DELETE FROM ""Users"" WHERE id = @Id;";
                var affected = await conn.ExecuteAsync(sql, new { Id = id });
                return affected > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al eliminar el usuario (Id: {id}): {ex.Message}", ex);
                throw;
            }
        }
    }
}