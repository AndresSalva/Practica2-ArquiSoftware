using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Mappers;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class ClientRepository : IUserRelationRepository<Client>
    {
        private readonly string _connectionString;

        public ClientRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Buscando cliente con ID: {id} en PostgreSQL con Dapper.");

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var userSql = "SELECT id, created_at AS CreatedAt, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth as DateBirth, ci, role FROM \"user\" WHERE id = @Id";
                var baseUser = await conn.QuerySingleOrDefaultAsync<User>(userSql, new { Id = id });

                if (baseUser == null || baseUser.Role != "Client")
                {
                    await RemoteLoggerSingleton.Instance.LogWarning($"No se encontró un usuario base con ID: {id} y rol 'Client'.");
                    return null;
                }

                var client = UserMapper.MapToUserDomain<Client>(baseUser);

                var detailsSql = "SELECT fitness_level AS FitnessLevel, initial_weight_kg AS InitialWeightKg, current_weight_kg AS CurrentWeightKg, emergency_contact_phone AS EmergencyContactPhone FROM client WHERE id_user = @Id";
                var detailsData = await conn.QuerySingleOrDefaultAsync<Client>(detailsSql, new { Id = id });

                if (detailsData != null)
                {
                    client.FitnessLevel = detailsData.FitnessLevel;
                    client.InitialWeightKg = detailsData.InitialWeightKg;
                    client.CurrentWeightKg = detailsData.CurrentWeightKg;
                    client.EmergencyContactPhone = detailsData.EmergencyContactPhone;
                }

                await RemoteLoggerSingleton.Instance.LogInfo($"Ensamblaje de cliente con ID: {id} completado.");
                return client;
            }
        }

        public async Task CreateAsync(Client client)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Iniciando creación de un nuevo cliente: {client.Name} con Dapper.");
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                using (var transaction = await conn.BeginTransactionAsync())
                {
                    try
                    {
                        var userSql =
                        @"INSERT INTO ""user""
                        (name, first_lastname, second_lastname, date_birth, ci, role)
                        VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @CI, @Role)
                        RETURNING id;";
                        var newUserId = await conn.QuerySingleAsync<int>(userSql, client, transaction);

                        client.IdUser = newUserId;
                        var clientSql =
                        @"INSERT INTO client
                        (id_user, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone)
                        VALUES (@IdUser, @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone);";
                        await conn.ExecuteAsync(clientSql, client, transaction);

                        await transaction.CommitAsync();
                        await RemoteLoggerSingleton.Instance.LogInfo($"Cliente con ID {newUserId} creado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await RemoteLoggerSingleton.Instance.LogError($"Error al crear cliente {client.Name}.", ex);
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateAsync(Client client)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql = @"UPDATE client
                SET fitness_level = @FitnessLevel,
                initial_weight_kg = @InitialWeightKg,
                current_weight_kg = @CurrentWeightKg,
                emergency_contact_phone = @EmergencyContactPhone
                WHERE id_user = @Id;";

            var parameters = new
            {
                client.Id,
                LastModification = DateTime.Now,
                client.FitnessLevel,
                client.InitialWeightKg,
                client.CurrentWeightKg,
                client.EmergencyContactPhone
            };

            var rows = await conn.ExecuteAsync(sql, parameters);
            return rows > 0;
        }

        public Task<bool> DeleteAsync(int id)
        {
            return Task.FromResult(false);
        }
    }
}