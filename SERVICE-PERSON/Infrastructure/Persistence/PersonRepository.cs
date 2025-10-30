using Dapper;
using Npgsql;
using ServiceCommon.Domain.Ports;
using ServicePerson.Domain.Entities;
using ServicePerson.Domain.Ports;

namespace ServicePerson.Infraestructure.Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly string _postgresString;
        private readonly IRemoteLogger _logger;
        private readonly IConnectionStringProvider _connectionStringProvider;

        public PersonRepository(IRemoteLogger logger, IConnectionStringProvider connectionStringProvider)
        {
            _logger = logger;
            _connectionStringProvider = connectionStringProvider;
            _postgresString = _connectionStringProvider.GetPostgresConnection();
        }

        public async Task<Person> CreateAsync(Person entity)
        {
            try
            {
                //await _logger.LogInfo($"Creating new Person: {entity.Name} {entity.FirstLastname}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"INSERT INTO person (name, first_lastname, second_lastname, date_birth, ci, ""role"", created_at, last_modification, is_active) VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive) RETURNING id;";

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                entity.Id = await conn.ExecuteScalarAsync<int>(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                //await _logger.LogError($"Error trying to create Person '{entity.Name}': {ex.Message}", ex);
                throw new Exception("Error creating Person", ex);
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            try
            {
                //await _logger.LogInfo($"Deleting Person: {id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"UPDATE person SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
                var affected = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
                return affected > 0;
            }
            catch (Exception ex)
            {
                //await _logger.LogError($"Error trying to eliminate Person (Id: {id}): {ex.Message}", ex);
                throw new Exception("Error creating Person", ex);
            }
        }

        public async Task<IEnumerable<Person>> GetAllAsync()
        {
            try
            {
                //await _logger.LogInfo("Trying to obtain full list");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"SELECT id, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth AS DateBirth, ci, ""role"" AS Role FROM person WHERE is_active = true;";
                return await conn.QueryAsync<Person>(sql);
            }
            catch (Exception ex)
            {
                //await _logger.LogError($"Error trying to obtain all Persons: {ex.Message}", ex);
                throw new Exception("Error creating Person", ex);
            }
        }

        public async Task<Person> GetByIdAsync(int id)
        {
            try
            {
                //await _logger.LogInfo($"Trying to obtain Person with id: {id} con Dapper.");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"SELECT id, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth AS DateBirth, ci, ""role"" AS Role, created_at AS CreatedAt, last_modification AS LastModification, is_active as IsActive FROM person WHERE id = @Id AND is_active = true;";
                return await conn.QuerySingleOrDefaultAsync<Person>(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                //await _logger.LogError($"Error trying to obtain Person with (Id: {id}): {ex.Message}", ex);
                throw new Exception("Error creating Person", ex);
            }
        }

        public async Task<Person> UpdateAsync(Person entity)
        {
            try
            {
                //await _logger.LogInfo($"Updating Person with id: {entity.Id}");
                using var conn = new NpgsqlConnection(_postgresString);
                var sql = @"UPDATE person SET name = @Name, first_lastname = @FirstLastname, second_lastname = @SecondLastname, date_birth = @DateBirth, ci = @CI, ""role"" = @Role, last_modification = @LastModification, is_active = @IsActive WHERE id = @Id;";

                entity.LastModification = DateTime.UtcNow;
                await conn.ExecuteAsync(sql, entity);
                return entity;
            }
            catch (Exception ex)
            {
                //await _logger.LogError($"Error trying to update Person (Id: {entity.Id}): {ex.Message}", ex);
                throw new Exception("Error creating Person", ex);
            }
        }
    }
}