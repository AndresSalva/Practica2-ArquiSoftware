using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly string _postgresString;

        public DetailUserRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<DetailsUser> CreateAsync(DetailsUser entity)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Creando detalle de usuario para UserId: {entity.IdUser}");
            using var conn = new NpgsqlConnection(_postgresString);

            var userExists = await conn.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM ""user"" WHERE id = @UserId AND is_active = true", new { UserId = entity.IdUser });
            if (!userExists) throw new ArgumentException($"El usuario con ID {entity.IdUser} no existe o no está activo.");

            var membershipExists = await conn.ExecuteScalarAsync<bool>(@"SELECT COUNT(1) FROM membership WHERE id = @MembershipId AND is_active = true", new { MembershipId = entity.IdMembership });
            if (!membershipExists) throw new ArgumentException($"La membresía con ID {entity.IdMembership} no existe o no está activa.");

            entity.CreatedAt = DateTime.UtcNow;
            entity.LastModification = DateTime.UtcNow;
            entity.IsActive = true;

            var sql = @"INSERT INTO details_user (id_user, id_membership, start_date, end_date, sessions_left, created_at, last_modification, is_active) VALUES (@IdUser, @IdMembership, @StartDate, @EndDate, @SessionsLeft, @CreatedAt, @LastModification, @IsActive) RETURNING id;";
            entity.Id = await conn.QuerySingleAsync<int>(sql, entity);
            return entity;
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            var sql = @"UPDATE details_user SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
            var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
            return affected > 0;
        }

        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_postgresString);
            var sql = @"SELECT id, id_user AS IdUser, id_membership AS IdMembership, start_date AS StartDate, end_date AS EndDate, sessions_left AS SessionsLeft, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive FROM details_user WHERE is_active = true;";
            return await conn.QueryAsync<DetailsUser>(sql);
        }

        public async Task<DetailsUser> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            var sql = @"SELECT id, id_user AS IdUser, id_membership AS IdMembership, start_date AS StartDate, end_date AS EndDate, sessions_left AS SessionsLeft, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive FROM details_user WHERE id = @Id;";
            return await conn.QueryFirstOrDefaultAsync<DetailsUser>(sql, new { Id = id });
        }

        public async Task<DetailsUser> UpdateAsync(DetailsUser entity)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            entity.LastModification = DateTime.UtcNow;
            var sql = @"UPDATE details_user SET id_user = @IdUser, id_membership = @IdMembership, start_date = @StartDate, end_date = @EndDate, sessions_left = @SessionsLeft, last_modification = @LastModification, is_active = @IsActive WHERE id = @Id;";
            var affectedRows = await conn.ExecuteAsync(sql, entity);
            if (affectedRows == 0) throw new KeyNotFoundException("No se encontró el detalle de usuario para actualizar.");
            return entity;
        }
    }
}