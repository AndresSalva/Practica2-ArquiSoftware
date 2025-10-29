using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;
using ServiceCommon.Infrastructure.Services;
using ServiceCommon.Domain.Ports;

namespace ServiceMembership.Infrastructure.Persistence;

public class DetailUserRepository : IDetailUserRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DetailUserRepository> _logger;

    public DetailUserRepository(IConnectionStringProvider connectionProvider, ILogger<DetailUserRepository> logger)
    {
        _connectionString = connectionProvider.GetPostgresConnection();
        _logger = logger;
    }

    public async Task<DetailsUser> CreateAsync(DetailsUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation("Creating details user for user {UserId} with membership {MembershipId}", entity.IdUser, entity.IdMembership);

        const string userExistsSql = @"SELECT COUNT(1) FROM ""user"" WHERE id = @UserId AND is_active = true;";
        const string membershipExistsSql = @"SELECT COUNT(1) FROM membership WHERE id = @MembershipId AND is_active = true;";
        const string insertSql = """
            INSERT INTO details_user (id_user, id_membership, start_date, end_date, sessions_left, created_at, last_modification, is_active)
            VALUES (@IdUser, @IdMembership, @StartDate, @EndDate, @SessionsLeft, @CreatedAt, @LastModification, @IsActive)
            RETURNING id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);

        var userExists = await conn.ExecuteScalarAsync<bool>(userExistsSql, new { UserId = entity.IdUser });
        if (!userExists)
        {
            throw new ArgumentException($"El usuario con identificador {entity.IdUser} no existe o está inactivo.");
        }

        var membershipExists = await conn.ExecuteScalarAsync<bool>(membershipExistsSql, new { MembershipId = entity.IdMembership });
        if (!membershipExists)
        {
            throw new ArgumentException($"La membresía con identificador {entity.IdMembership} no existe o está inactivo.");
        }

        entity.CreatedAt = DateTime.UtcNow;
        entity.LastModification = DateTime.UtcNow;
        entity.IsActive = true;

        var newId = await conn.QuerySingleAsync<int>(insertSql, entity);
        entity.Id = newId;
        return entity;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        _logger.LogInformation("Soft deleting details user {DetailsUserId}", id);

        const string sql = """
            UPDATE details_user
            SET is_active = false,
                last_modification = @LastModification
            WHERE id = @Id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
        return affected > 0;
    }

    public async Task<IEnumerable<DetailsUser>> GetAllAsync()
    {
        _logger.LogInformation("Fetching active details users");

        const string sql = """
            SELECT id,
                   id_user AS IdUser,
                   id_membership AS IdMembership,
                   start_date AS StartDate,
                   end_date AS EndDate,
                   sessions_left AS SessionsLeft,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   is_active AS IsActive
            FROM details_user
            WHERE is_active = true;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<DetailsUser>(sql);
    }

    public async Task<DetailsUser?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching details user {DetailsUserId}", id);

        const string sql = """
            SELECT id,
                   id_user AS IdUser,
                   id_membership AS IdMembership,
                   start_date AS StartDate,
                   end_date AS EndDate,
                   sessions_left AS SessionsLeft,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   is_active AS IsActive
            FROM details_user
            WHERE id = @Id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<DetailsUser>(sql, new { Id = id });
    }

    public async Task<DetailsUser?> UpdateAsync(DetailsUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation("Updating details user {DetailsUserId}", entity.Id);

        const string sql = """
            UPDATE details_user
            SET id_user = @IdUser,
                id_membership = @IdMembership,
                start_date = @StartDate,
                end_date = @EndDate,
                sessions_left = @SessionsLeft,
                last_modification = @LastModification,
                is_active = @IsActive
            WHERE id = @Id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);

        entity.LastModification = DateTime.UtcNow;

        var affectedRows = await conn.ExecuteAsync(sql, entity);
        return affectedRows > 0 ? entity : null;
    }
}


