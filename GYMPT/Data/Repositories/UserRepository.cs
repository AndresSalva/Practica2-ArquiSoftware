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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // ✅ READ ALL
        public async Task<IEnumerable<UserData>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de usuarios con Dapper.");
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"SELECT id,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   ""isActive"" AS IsActive,
                   name AS Name,
                   first_lastname AS FirstLastname,
                   second_lastname AS SecondLastname,
                   date_birth AS DateBirth,
                   ""CI"" AS CI,
                   role AS Role
            FROM ""User""
            WHERE ""isActive"" = true;";


            return await conn.QueryAsync<UserData>(sql);
        }

        // ✅ READ ONE
        public async Task<UserData> GetByIdAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"SELECT id,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   ""isActive"" AS IsActive,
                   name AS Name,
                   first_lastname AS FirstLastname,
                   second_lastname AS SecondLastname,
                   date_birth AS DateBirth,
                   ""CI"" AS CI,
                   role AS Role
            FROM ""User""
            WHERE id = @Id;";


            return await conn.QueryFirstOrDefaultAsync<UserData>(sql, new { Id = id });
        }

        // ✅ CREATE
        public async Task<long> CreateAsync(UserData user)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"INSERT INTO ""User"" 
                            (created_at, isActive, name, first_lastname, second_lastname, date_birth, CI, role)
                        VALUES 
                            (@CreatedAt, @IsActive, @Name, @FirstLastname, @SecondLastname, @DateBirth, @CI, @Role)
                        RETURNING id;";

            var parameters = new
            {
                CreatedAt = DateTime.Now,
                IsActive = true,
                user.Name,
                user.FirstLastname,
                user.SecondLastname,
                user.DateBirth,
                user.CI,
                user.Role
            };

            return await conn.ExecuteScalarAsync<long>(sql, parameters);
        }

        // ✅ UPDATE
        public async Task<bool> UpdateAsync(UserData user)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"UPDATE ""User"" 
                        SET name = @Name,
                            first_lastname = @FirstLastname,
                            second_lastname = @SecondLastname,
                            date_birth = @DateBirth,
                            ""CI"" = @CI,
                            ""isActive"" = @IsActive,
                            last_modification = @LastModification
                        WHERE id = @Id;";


            var parameters = new
            {
                user.Id,
                user.Name,
                user.FirstLastname,
                user.SecondLastname,
                user.DateBirth,
                user.CI,
                IsActive = user.IsActive ?? true,
                LastModification = DateTime.Now
            };


            var rows = await conn.ExecuteAsync(sql, parameters);
            return rows > 0;
        }

        // ✅ DELETE LÓGICO
        public async Task<bool> DeleteAsync(long id)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"UPDATE ""User"" 
                        SET ""isActive"" = false, last_modification = @LastModification
                        WHERE id = @Id;";

            var parameters = new
            {
                Id = id,
                LastModification = DateTime.Now
            };

            var rows = await conn.ExecuteAsync(sql, parameters);
            return rows > 0;
        }
    }
}
