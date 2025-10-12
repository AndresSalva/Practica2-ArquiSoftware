using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class ClientRepository : IRepository<Client>
    {
        private readonly string _postgresString;

        public ClientRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string sql = @"
                SELECT 
                    u.id AS Id,
                    u.name AS Name,
                    u.first_lastname AS FirstLastname,
                    u.second_lastname AS SecondLastname,
                    u.date_birth AS DateBirth,
                    u.ci AS Ci,
                    u.role AS Role,
                    u.created_at AS CreatedAt,
                    u.last_modification AS LastModification,
                    u.is_active AS IsActive,
                    c.fitness_level AS FitnessLevel,
                    c.initial_weight_kg AS InitialWeightKg,
                    c.current_weight_kg AS CurrentWeightKg,
                    c.emergency_contact_phone AS EmergencyContactPhone
                FROM ""user"" u
                INNER JOIN client c ON u.id = c.id_user
                WHERE u.id = @Id AND u.is_active = true;";

            return await conn.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string sql = @"
                SELECT 
                    u.id AS Id,
                    u.name AS Name,
                    u.first_lastname AS FirstLastname,
                    u.second_lastname AS SecondLastname,
                    u.date_birth AS DateBirth,
                    u.ci AS Ci,
                    u.role AS Role,
                    u.created_at AS CreatedAt,
                    u.last_modification AS LastModification,
                    u.is_active AS IsActive,
                    c.fitness_level AS FitnessLevel,
                    c.initial_weight_kg AS InitialWeightKg,
                    c.current_weight_kg AS CurrentWeightKg,
                    c.emergency_contact_phone AS EmergencyContactPhone
                FROM ""user"" u
                INNER JOIN client c ON u.id = c.id_user
                WHERE u.is_active = true AND u.role = 'Client';";

            return await conn.QueryAsync<Client>(sql);
        }

        public async Task<Client> CreateAsync(Client entity)
        {

            await RemoteLoggerSingleton.Instance.LogInfo($"Creating client: {entity.Name}");
            using var conn = new NpgsqlConnection(_postgresString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                entity.Role = "Client";
                var userSql = @"INSERT INTO ""user"" (name, first_lastname, second_lastname, date_birth, ci, role, created_at, last_modification, is_active) VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive) RETURNING id;";
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                var newUserId = await conn.QuerySingleAsync<int>(userSql, entity, transaction);
                entity.Id = newUserId;
                entity.IdUser = newUserId;

                var clientSql = @"INSERT INTO client (id_user, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone) VALUES (@IdUser, @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone);";
                await conn.ExecuteAsync(clientSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await RemoteLoggerSingleton.Instance.LogError($"Error creating client {entity.Name}.", ex);
                throw;
            }
        }

        public async Task<Client> UpdateAsync(Client entity)
        {

            using var conn = new NpgsqlConnection(_postgresString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                entity.LastModification = DateTime.UtcNow;
                var userSql = @"UPDATE ""user"" SET name = @Name, first_lastname = @FirstLastname, second_lastname = @SecondLastname, date_birth = @DateBirth, ci = @Ci, last_modification = @LastModification WHERE id = @Id;";
                await conn.ExecuteAsync(userSql, entity, transaction);

                var clientSql = @"UPDATE client SET fitness_level = @FitnessLevel, initial_weight_kg = @InitialWeightKg, current_weight_kg = @CurrentWeightKg, emergency_contact_phone = @EmergencyContactPhone WHERE id_user = @Id;";
                await conn.ExecuteAsync(clientSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await RemoteLoggerSingleton.Instance.LogError($"Error updating client {entity.Id}.", ex);
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {

            using var conn = new NpgsqlConnection(_postgresString);
            var sql = @"UPDATE ""user"" SET is_active = false, last_modification = @LastModification WHERE id = @Id;";
            var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
            return affectedRows > 0;
        }
    }
}