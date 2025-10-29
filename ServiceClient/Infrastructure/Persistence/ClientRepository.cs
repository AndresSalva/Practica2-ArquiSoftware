using Dapper;
using Npgsql;
using ServiceCommon.Domain.Ports;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;

public class ClientRepository : IClientRepository
{
    private readonly string _connString;
    private readonly IConnectionStringProvider _connectionProvider;

    public ClientRepository(IConnectionStringProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        _connString = _connectionProvider.GetPostgresConnection();
    }

    public async Task<Client> CreateAsync(Client client)
    {
        using var conn = new NpgsqlConnection(_connString);
        using var transaction = conn.BeginTransaction();
        try
        {
            const string personSql = @"
                INSERT INTO public.person (name, first_lastname, second_lastname, date_birth, ci, is_active, created_at)
                VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @IsActive, @CreatedAt) RETURNING id;";

            var personId = await conn.ExecuteScalarAsync<int>(personSql, client, transaction);
            client.Id = personId;

            const string clientSql = @"
                INSERT INTO public.client (id_person, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone)
                VALUES (@Id, @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone);";

            await conn.ExecuteAsync(clientSql, client, transaction);

            transaction.Commit();
            return client;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        using var conn = new NpgsqlConnection(_connString);
        const string sql = @"
            SELECT p.id AS Id, p.created_at AS CreatedAt, p.last_modification AS LastModification, p.is_active AS IsActive, p.name AS Name, p.first_lastname AS FirstLastname, p.second_lastname AS SecondLastname, p.date_birth AS DateBirth, p.ci AS Ci, 'Client' AS Role, c.fitness_level AS FitnessLevel, c.initial_weight_kg AS InitialWeightKg, c.current_weight_kg AS CurrentWeightKg, c.emergency_contact_phone AS EmergencyContactPhone
            FROM public.person p INNER JOIN public.client c ON p.id = c.id_person;";
        return await conn.QueryAsync<Client>(sql);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        using var conn = new NpgsqlConnection(_connString);
        const string sql = @"
            SELECT p.id AS Id, p.created_at AS CreatedAt, p.last_modification AS LastModification, p.is_active AS IsActive, p.name AS Name, p.first_lastname AS FirstLastname, p.second_lastname AS SecondLastname, p.date_birth AS DateBirth, p.ci AS Ci, 'Client' AS Role, c.fitness_level AS FitnessLevel, c.initial_weight_kg AS InitialWeightKg, c.current_weight_kg AS CurrentWeightKg, c.emergency_contact_phone AS EmergencyContactPhone
            FROM public.person p INNER JOIN public.client c ON p.id = c.id_person
            WHERE p.id = @Id;";
        return await conn.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
    }

    public async Task<Client?> UpdateAsync(Client client)
    {
        using var conn = new NpgsqlConnection(_connString);
        client.LastModification = System.DateTime.UtcNow;
        const string sql = @"
            UPDATE public.person SET name = @Name, first_lastname = @FirstLastname, second_lastname = @SecondLastname, date_birth = @DateBirth, ci = @Ci, last_modification = @LastModification WHERE id = @Id;
            UPDATE public.client SET fitness_level = @FitnessLevel, current_weight_kg = @CurrentWeightKg, emergency_contact_phone = @EmergencyContactPhone WHERE id_person = @Id;";
        var affectedRows = await conn.ExecuteAsync(sql, client);
        return affectedRows > 0 ? client : null;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        using var conn = new NpgsqlConnection(_connString);
        const string sql = "DELETE FROM public.person WHERE id = @Id;";
        var affectedRows = await conn.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}