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
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly string _connectionString;

        public DetailUserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Obteniendo todos los detalles de usuarios.");
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"SELECT id, id_user AS IdUser, id_membership AS IdMembership,
                               start_date AS StartDate, end_date AS EndDate,
                               sessions_left AS SessionsLeft, created_at AS CreatedAt,
                               last_modification AS LastModification, ""isActive"" as IsActive
                        FROM ""Details_user""";
            return await conn.QueryAsync<DetailsUser>(sql);
        }

        public async Task<DetailsUser> CreateAsync(DetailsUser entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            entity.CreatedAt = DateTime.UtcNow;
            entity.LastModification = DateTime.UtcNow;
            entity.IsActive = true;

            var sql = @"
        INSERT INTO ""Details_user"" 
        (id_user, id_membership, start_date, end_date, sessions_left, created_at, last_modification, ""isActive"")
        VALUES (@IdUser, @IdMembership, @StartDate, @EndDate, @SessionsLeft, @CreatedAt, @LastModification, @IsActive)
        RETURNING id;";

            entity.Id = await conn.QuerySingleAsync<int>(sql, entity);
            return entity;
        }

        public async Task<DetailsUser> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"SELECT id, id_user AS IdUser, id_membership AS IdMembership,
                       start_date AS StartDate, end_date AS EndDate,
                       sessions_left AS SessionsLeft, created_at AS CreatedAt,
                       last_modification AS LastModification, ""isActive"" as IsActive
                FROM ""Details_user""
                WHERE id = @Id";

            return await conn.QueryFirstOrDefaultAsync<DetailsUser>(sql, new { Id = id });
        }


        public async Task<DetailsUser> UpdateAsync(DetailsUser entity)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            entity.LastModification = DateTime.UtcNow;

            var sql = @"
                UPDATE ""Details_user"" 
                SET id_user = @IdUser,
                    id_membership = @IdMembership,
                    start_date = @StartDate,
                    end_date = @EndDate,
                    sessions_left = @SessionsLeft,
                    last_modification = @LastModification,
                    ""isActive"" = @IsActive
                WHERE id = @Id;";

            var affectedRows = await conn.ExecuteAsync(sql, entity);
            if (affectedRows == 0)
                throw new KeyNotFoundException("No se encontró el detalle de usuario para actualizar.");

            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_connectionString);
            var sql = @"DELETE FROM ""Details_user"" WHERE id = @Id;";
            var affected = await conn.ExecuteAsync(sql, new { Id = id });
            return affected > 0;
        }
    }
}
