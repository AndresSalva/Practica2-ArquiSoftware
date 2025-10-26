using Dapper;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceMembership.Domain.Entities;
using ServiceMembership.Domain.Ports;
using ServiceMembership.Infrastructure.Providers;

namespace ServiceMembership.Infrastructure.Persistence;

public class DetailMembershipRepository : IDetailMembershipRepository
{
    private readonly string _connectionString;
    private readonly ILogger<DetailMembershipRepository> _logger;

    public DetailMembershipRepository(IMembershipConnectionProvider connectionProvider, ILogger<DetailMembershipRepository> logger)
    {
        ArgumentNullException.ThrowIfNull(connectionProvider, nameof(connectionProvider));
        ArgumentNullException.ThrowIfNull(logger, nameof(logger));

        var connectionString = connectionProvider.GetConnectionString();
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("El repositorio de detalles de membresía requiere una cadena de conexión válida.");
        }

        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<DetailsMembership> CreateAsync(DetailsMembership entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation("Creating membership-detail link for membership {MembershipId} and discipline {DisciplineId}", entity.IdMembership, entity.IdDiscipline);

        const string membershipExistsSql = @"SELECT COUNT(1) FROM membership WHERE id = @IdMembership AND is_active = true;";
        const string disciplineExistsSql = @"SELECT COUNT(1) FROM discipline WHERE id = @IdDiscipline AND is_active = true;";
        const string combinationExistsSql = @"SELECT COUNT(1) FROM details_membership WHERE id_membership = @IdMembership AND id_discipline = @IdDiscipline;";
        const string insertSql = @"INSERT INTO details_membership (id_membership, id_discipline) VALUES (@IdMembership, @IdDiscipline);";

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var membershipExists = await conn.ExecuteScalarAsync<bool>(membershipExistsSql, new { entity.IdMembership });
        if (!membershipExists)
        {
            throw new ArgumentException($"La membresía con identificador {entity.IdMembership} no existe o está inactiva.");
        }

        var disciplineExists = await conn.ExecuteScalarAsync<bool>(disciplineExistsSql, new { entity.IdDiscipline });
        if (!disciplineExists)
        {
            throw new ArgumentException($"La disciplina con identificador {entity.IdDiscipline} no existe o está inactiva.");
        }

        var combinationExists = await conn.ExecuteScalarAsync<bool>(combinationExistsSql, new { entity.IdMembership, entity.IdDiscipline });
        if (combinationExists)
        {
            throw new InvalidOperationException("La membresía indicada ya contiene la disciplina especificada.");
        }

        await conn.ExecuteAsync(insertSql, entity);
        return entity;
    }

    public async Task<bool> DeleteByIdsAsync(short membershipId, short disciplineId)
    {
        _logger.LogInformation("Deleting membership-detail link for membership {MembershipId} and discipline {DisciplineId}", membershipId, disciplineId);

        const string sql = """
            DELETE FROM details_membership
            WHERE id_membership = @IdMembership
              AND id_discipline = @IdDiscipline;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var affected = await conn.ExecuteAsync(sql, new { IdMembership = membershipId, IdDiscipline = disciplineId });
        return affected > 0;
    }

    public async Task<bool> DeleteByMembershipIdAsync(short membershipId)
    {
        _logger.LogInformation("Deleting all membership-detail links for membership {MembershipId}", membershipId);

        const string sql = """
            DELETE FROM details_membership
            WHERE id_membership = @IdMembership;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        var affected = await conn.ExecuteAsync(sql, new { IdMembership = membershipId });
        return affected > 0;
    }

    public async Task<IEnumerable<DetailsMembership>> GetAllAsync()
    {
        _logger.LogInformation("Fetching all membership-detail links");

        const string sql = """
            SELECT id_membership AS IdMembership,
                   id_discipline AS IdDiscipline
            FROM details_membership;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<DetailsMembership>(sql);
    }

    public async Task<IEnumerable<DetailsMembership>> GetByMembershipIdAsync(short membershipId)
    {
        _logger.LogInformation("Fetching membership-detail links for membership {MembershipId}", membershipId);

        const string sql = """
            SELECT id_membership AS IdMembership,
                   id_discipline AS IdDiscipline
            FROM details_membership
            WHERE id_membership = @IdMembership;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QueryAsync<DetailsMembership>(sql, new { IdMembership = membershipId });
    }

    public async Task<DetailsMembership?> GetByIdsAsync(short membershipId, short disciplineId)
    {
        _logger.LogInformation("Fetching membership-detail link for membership {MembershipId} and discipline {DisciplineId}", membershipId, disciplineId);

        const string sql = """
            SELECT id_membership AS IdMembership,
                   id_discipline AS IdDiscipline
            FROM details_membership
            WHERE id_membership = @IdMembership
              AND id_discipline = @IdDiscipline;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        return await conn.QuerySingleOrDefaultAsync<DetailsMembership>(sql, new { IdMembership = membershipId, IdDiscipline = disciplineId });
    }

    public async Task<DetailsMembership?> UpdateAsync(short membershipId, short disciplineId, DetailsMembership entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        _logger.LogInformation(
            "Updating membership-detail link from membership {MembershipId} and discipline {DisciplineId} to membership {NewMembershipId} and discipline {NewDisciplineId}",
            membershipId,
            disciplineId,
            entity.IdMembership,
            entity.IdDiscipline);

        const string existingCombinationSql = @"SELECT COUNT(1) FROM details_membership WHERE id_membership = @IdMembership AND id_discipline = @IdDiscipline;";
        const string membershipExistsSql = @"SELECT COUNT(1) FROM membership WHERE id = @IdMembership AND is_active = true;";
        const string disciplineExistsSql = @"SELECT COUNT(1) FROM discipline WHERE id = @IdDiscipline AND is_active = true;";
        const string updateSql = """
            UPDATE details_membership
            SET id_membership = @NewIdMembership,
                id_discipline = @NewIdDiscipline
            WHERE id_membership = @IdMembership
              AND id_discipline = @IdDiscipline;
            """;

        await using var conn = new NpgsqlConnection(_connectionString);
        await conn.OpenAsync();

        var currentExists = await conn.ExecuteScalarAsync<bool>(existingCombinationSql, new { IdMembership = membershipId, IdDiscipline = disciplineId });
        if (!currentExists)
        {
            return null;
        }

        var membershipExists = await conn.ExecuteScalarAsync<bool>(membershipExistsSql, new { IdMembership = entity.IdMembership });
        if (!membershipExists)
        {
            throw new ArgumentException($"La membresía con identificador {entity.IdMembership} no existe o está inactiva.");
        }

        var disciplineExists = await conn.ExecuteScalarAsync<bool>(disciplineExistsSql, new { IdDiscipline = entity.IdDiscipline });
        if (!disciplineExists)
        {
            throw new ArgumentException($"La disciplina con identificador {entity.IdDiscipline} no existe o está inactiva.");
        }

        var isSameCombination = membershipId == entity.IdMembership && disciplineId == entity.IdDiscipline;
        if (!isSameCombination)
        {
            var targetExists = await conn.ExecuteScalarAsync<bool>(existingCombinationSql, new { IdMembership = entity.IdMembership, IdDiscipline = entity.IdDiscipline });
            if (targetExists)
            {
                throw new InvalidOperationException("La membresía de destino ya contiene la disciplina especificada.");
            }
        }

        var affectedRows = await conn.ExecuteAsync(updateSql, new
        {
            IdMembership = membershipId,
            IdDiscipline = disciplineId,
            NewIdMembership = entity.IdMembership,
            NewIdDiscipline = entity.IdDiscipline
        });

        return affectedRows > 0 ? entity : null;
    }
}



