// Ruta: ServiceClient/Infrastructure/Persistence/UserRepository.cs
using Dapper;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

public class UserRepository : IUserRepository
{
    private readonly IClientConnectionProvider _connectionProvider;

    public UserRepository(IClientConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        const string sql = @"
            SELECT p.id AS Id, p.name AS Name, p.first_lastname AS FirstLastname, p.second_lastname AS SecondLastname, p.date_birth AS DateBirth, p.ci AS Ci, u.role AS Role, p.created_at AS CreatedAt, p.last_modification AS LastModification, p.is_active AS IsActive
            FROM public.person p INNER JOIN public.user u ON p.id = u.id_person;";

        // CORRECCIÓN: Se llama al método definido en la interfaz.
        using var conn = _connectionProvider.CreateConnection();
        return await conn.QueryAsync<User>(sql);
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        const string sql = @"
            SELECT p.id AS Id, p.name AS Name, p.first_lastname AS FirstLastname, p.second_lastname AS SecondLastname, p.date_birth AS DateBirth, p.ci AS Ci, u.role AS Role, p.created_at AS CreatedAt, p.last_modification AS LastModification, p.is_active AS IsActive
            FROM public.person p INNER JOIN public.user u ON p.id = u.id_person
            WHERE p.id = @Id;";

        // CORRECCIÓN: Se llama al método definido en la interfaz.
        using var conn = _connectionProvider.CreateConnection();
        return await conn.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> CreateAsync(User user)
    {
        // CORRECCIÓN: Se llama al método definido en la interfaz.
        using var conn = _connectionProvider.CreateConnection();
        conn.Open();
        using var transaction = conn.BeginTransaction();
        try
        {
            const string personSql = @"
                INSERT INTO public.person (name, first_lastname, second_lastname, date_birth, ci, is_active, created_at)
                VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @IsActive, @CreatedAt) RETURNING id;";
            var personId = await conn.ExecuteScalarAsync<int>(personSql, user, transaction);
            user.Id = personId;

            const string userSql = @"
                INSERT INTO public.user (id_person, role) -- Añade otros campos como password, email si son necesarios
                VALUES (@Id, @Role);";
            await conn.ExecuteAsync(userSql, user, transaction);

            transaction.Commit();
            return user;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<User?> UpdateAsync(User user)
    {
        const string sql = @"
            UPDATE public.person SET name = @Name, first_lastname = @FirstLastname, last_modification = @LastModification WHERE id = @Id;
            UPDATE public.user SET role = @Role WHERE id_person = @Id;";

        // CORRECCIÓN: Se llama al método definido en la interfaz.
        using var conn = _connectionProvider.CreateConnection();
        var affectedRows = await conn.ExecuteAsync(sql, user);
        return affectedRows > 0 ? user : null;
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        const string sql = "DELETE FROM public.person WHERE id = @Id;";

        // CORRECCIÓN: Se llama al método definido en la interfaz.
        using var conn = _connectionProvider.CreateConnection();
        var affectedRows = await conn.ExecuteAsync(sql, new { Id = id });
        return affectedRows > 0;
    }
}