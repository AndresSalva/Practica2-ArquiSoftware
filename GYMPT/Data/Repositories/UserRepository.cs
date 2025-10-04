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
    public class UserRepository : IRepository<UserData> // Esta es la clase que causaba el error CS0535
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
                // Ajusta los nombres de las columnas SQL para que coincidan con tu base de datos
                // y usa alias (AS) para que Dapper mapee correctamente a las propiedades de UserData
                var sql = @"SELECT id, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                                   date_birth AS DateBirth, ci AS CI, role AS Role,
                                   created_at AS CreatedAt, last_modification AS LastModification, ""isActive"" as IsActive
                            FROM ""Users"""; // <-- Asegúrate de que "Users" sea el nombre correcto de tu tabla
                return await conn.QueryAsync<UserData>(sql);
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al obtener todos los usuarios: {ex.Message}", ex);
                throw;
            }
        }

        // Add this method to implement IRepository<UserData>.GetByIdAsync(int)
        public async Task<UserData> GetByIdAsync(int id)
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
                               (name, first_lastname, second_lastname, date_birth, ci, role, created_at, ""isActive"")
                            VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @CI, @Role, NOW(), @IsActive)
                            RETURNING id;";
                // Asumo que tu columna 'id' es de tipo BIGINT en la BD, por eso uso long
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
                                last_modification = NOW(),
                                ""isActive"" = @IsActive
                            WHERE id = @Id;";
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar el usuario (Id: {entity.Id}): {ex.Message}", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                await RemoteLoggerSingleton.Instance.LogInfo($"Eliminando usuario con Id: {id}");
                using var conn = new NpgsqlConnection(_connectionString);
                var sql = @"DELETE FROM ""Users"" WHERE id = @Id;"; // <-- Asegúrate de que "Users" sea el nombre correcto
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