using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;
using ServiceMembership.Infrastructure.Providers;

namespace ServiceMembership.Infrastructure.Persistence;

public class MembershipRepository : IMembershipRepository
{
    private readonly string _connectionString;
    private readonly ILogger<MembershipRepository> _logger;

    public MembershipRepository(IMembershipConnectionProvider connectionProvider, ILogger<MembershipRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        var connectionString = connectionProvider.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("El módulo de membresías requiere una cadena de conexión válida.");
        }

        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<Membership> CreateAsync(Membership entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation("Creating membership {MembershipName}", entity.Name);

        const string sql = """
            INSERT INTO membership (name, price, description, monthly_sessions, created_at, last_modification, is_active)
            VALUES (@Name, @Price, @Description, @MonthlySessions, @CreatedAt, @LastModification, @IsActive)
            RETURNING id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);

        entity.CreatedAt = DateTime.UtcNow;
        entity.LastModification = DateTime.UtcNow;
        entity.IsActive = true;

        var newId = await conn.QuerySingleAsync<short>(sql, entity);
        entity.Id = newId;

        return entity;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        _logger.LogInformation("Soft deleting membership {MembershipId}", id);

        const string sql = """
            UPDATE membership
            SET is_active = false,
                last_modification = @LastModification
            WHERE id = @Id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);

        var affectedRows = await conn.ExecuteAsync(sql, new
        {
            Id = id,
            LastModification = DateTime.UtcNow
        });

        return affectedRows > 0;
    }

    public async Task<IEnumerable<Membership>> GetAllAsync()
    {
        _logger.LogInformation("Fetching active membership list");

        const string sql = """
            SELECT id,
                   name,
                   price,
                   description,
                   monthly_sessions AS MonthlySessions,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   is_active AS IsActive
            FROM membership
            WHERE is_active = true;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<Membership>(sql);
    }

    public async Task<Membership?> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching membership {MembershipId}", id);

        const string sql = """
            SELECT id,
                   name,
                   price,
                   description,
                   monthly_sessions AS MonthlySessions,
                   created_at AS CreatedAt,
                   last_modification AS LastModification,
                   is_active AS IsActive
            FROM membership
            WHERE id = @Id;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<Membership>(sql, new { Id = id });
    }

    public async Task<Membership?> UpdateAsync(Membership entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation("Updating membership {MembershipId}", entity.Id);

        const string sql = """
            UPDATE membership
            SET name = @Name,
                price = @Price,
                description = @Description,
                monthly_sessions = @MonthlySessions,
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


