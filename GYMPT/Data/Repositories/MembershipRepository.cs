using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class MembershipRepository : IRepository<Membership>
    {
        private readonly string _connectionString;

        public MembershipRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Membership> CreateAsync(Membership entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creando nueva membresía: {entity.Name}, con ID: {entity.Id}");
            var sql = @"
                INSERT INTO ""Membership"" (name, price, description, monthly_sessions, created_at, last_modification, ""isActive"")
                VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @LastModification, @IsActive)
                RETURNING id;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true; 

                var newId = await conn.QuerySingleAsync<int>(sql, entity);
                entity.Id = newId;
            }
            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Dando de baja membresía con ID: {id}.");
            var sql = @"UPDATE ""Membership"" SET ""isActive"" = false, last_modification = @LastModification WHERE id = @Id;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de membresías con Dapper.");
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT id, name, price, description, monthly_sessions AS MonthlySessions, created_at AS CreatedAt, last_modification AS LastModification, \"isActive\" as IsActive FROM \"Membership\"";
                return await conn.QueryAsync<Membership>(sql);
            }
        }
        public async Task<Membership> UpdateAsync(Membership entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Actualizando membresía: {entity.Name}, con ID: {entity.Id}.");
            var sql = @"UPDATE ""Membership""SET name = @Name,price = @Price,description = @Description,monthly_sessions = @MonthlySessions,last_modification = @LastModification,""isActive"" = true WHERE id = @Id;";

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                entity.LastModification = DateTime.UtcNow;

                var affectedRows = await conn.ExecuteAsync(sql, entity);
                if (affectedRows == 0)
                {
                    throw new KeyNotFoundException("No se encontró una membresía con el ID proporcionado para actualizar.");
                }
            }
            return entity;
        }

    }
}