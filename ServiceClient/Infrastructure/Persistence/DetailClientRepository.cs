using Dapper;
using Microsoft.Extensions.Logging;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure.Persistence
{
    public class DetailClientRepository : IDetailClientRepository
    {
        private readonly IClientConnectionProvider _connectionProvider;
        private readonly ILogger<DetailClientRepository> _logger;

        public DetailClientRepository(IClientConnectionProvider connectionProvider, ILogger<DetailClientRepository> logger)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            _logger.LogInformation("Buscando cliente con ID {ClientId}", id);
            const string sql = @"
                SELECT id AS Id, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive,
                       name AS Name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                       date_birth AS DateBirth, ci AS Ci, role AS Role, fitness_level AS FitnessLevel,
                       initial_weight_kg AS InitialWeightKg, current_weight_kg AS CurrentWeightKg,
                       emergency_contact_phone AS EmergencyContactPhone
                FROM Users WHERE id = @Id AND role = 'Client';";

            using var conn = _connectionProvider.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todos los clientes");
            const string sql = @"
                SELECT id AS Id, created_at AS CreatedAt, last_modification AS LastModification, is_active AS IsActive,
                       name AS Name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                       date_birth AS DateBirth, ci AS Ci, role AS Role, fitness_level AS FitnessLevel,
                       initial_weight_kg AS InitialWeightKg, current_weight_kg AS CurrentWeightKg,
                       emergency_contact_phone AS EmergencyContactPhone
                FROM Users WHERE role = 'Client';";

            using var conn = _connectionProvider.CreateConnection();
            return await conn.QueryAsync<Client>(sql);
        }

        public async Task<Client> FindByCiAsync(string ci)
        {
            _logger.LogInformation("Buscando cliente por CI {ClientCI}", ci);
            const string sql = "SELECT * FROM Users WHERE ci = @Ci";
            using var conn = _connectionProvider.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Client>(sql, new { Ci = ci });
        }
    }
}