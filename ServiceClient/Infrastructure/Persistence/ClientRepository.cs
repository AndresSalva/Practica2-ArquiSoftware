using Dapper;
using Microsoft.Extensions.Logging;
using ServiceClient.Domain.Entities;
using ServiceClient.Domain.Ports;
using ServiceClient.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace ServiceClient.Infrastructure.Persistence
{
    public class ClientRepository : IClientRepository
    {
        private readonly IClientConnectionProvider _connectionProvider;
        private readonly ILogger<ClientRepository> _logger;

        public ClientRepository(IClientConnectionProvider connectionProvider, ILogger<ClientRepository> logger)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private IDbConnection CreateConnection() => _connectionProvider.CreateConnection();

        public async Task<Client> CreateAsync(Client entity)
        {
            _logger.LogInformation("Iniciando transacción para crear un nuevo cliente con CI {ClientCI}", entity.Ci);

            using var conn = CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                // Paso 1: Insertar en la tabla 'user' y obtener el nuevo ID.
                const string userSql = """
                    INSERT INTO "user" (name, first_lastname, second_lastname, date_birth, ci, "role", 
                                       created_at, last_modification, is_active)
                    VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, 'Client',
                            @CreatedAt, @LastModification, @IsActive)
                    RETURNING id;
                    """;

                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                var newUserId = await conn.ExecuteScalarAsync<int>(userSql, entity, transaction);
                entity.Id = newUserId;

                // Paso 2: Usar el nuevo ID para insertar en la tabla 'client'.
                const string clientSql = """
                    INSERT INTO client (id_user, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone)
                    VALUES (@IdUser, @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone);
                    """;

                await conn.ExecuteAsync(clientSql, new
                {
                    IdUser = entity.Id,
                    entity.FitnessLevel,
                    entity.InitialWeightKg,
                    entity.CurrentWeightKg,
                    entity.EmergencyContactPhone
                }, transaction);

                // Si todo fue exitoso, confirma la transacción.
                transaction.Commit();
                _logger.LogInformation("Cliente con ID {ClientId} creado exitosamente.", entity.Id);
                return entity;
            }
            catch (Exception ex)
            {
                // Si algo falla, revierte todos los cambios.
                _logger.LogError(ex, "Error al crear el cliente. Revirtiendo transacción.");
                transaction.Rollback();
                throw;
            }
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo la lista de clientes activos desde la vista 'client_view'");
            // Usamos la VISTA que ya une las dos tablas por nosotros.
            const string sql = "SELECT * FROM public.client_view WHERE IsActive = true;";

            using var conn = CreateConnection();
            return await conn.QueryAsync<Client>(sql);
        }

        public async Task<Client?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo cliente con Id: {ClientId} desde la vista 'client_view'", id);
            // Usamos la VISTA para obtener un cliente específico.
            const string sql = "SELECT * FROM public.client_view WHERE ClientId = @Id;";

            using var conn = CreateConnection();
            return await conn.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public async Task<Client?> UpdateAsync(Client entity)
        {
            _logger.LogInformation("Iniciando transacción para actualizar cliente con ID {ClientId}", entity.Id);

            using var conn = CreateConnection();
            conn.Open();
            using var transaction = conn.BeginTransaction();

            try
            {
                entity.LastModification = DateTime.UtcNow;

                // Paso 1: Actualizar la tabla 'user'
                const string userSql = """
                    UPDATE "user" SET
                        name = @Name,
                        first_lastname = @FirstLastname,
                        second_lastname = @SecondLastname,
                        date_birth = @DateBirth,
                        ci = @Ci,
                        last_modification = @LastModification,
                        is_active = @IsActive
                    WHERE id = @Id;
                    """;
                await conn.ExecuteAsync(userSql, entity, transaction);

                // Paso 2: Actualizar la tabla 'client'
                const string clientSql = """
                    UPDATE client SET
                        fitness_level = @FitnessLevel,
                        initial_weight_kg = @InitialWeightKg,
                        current_weight_kg = @CurrentWeightKg,
                        emergency_contact_phone = @EmergencyContactPhone
                    WHERE id_user = @Id;
                    """;
                var affectedRows = await conn.ExecuteAsync(clientSql, entity, transaction);

                transaction.Commit();
                return affectedRows > 0 ? entity : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar el cliente. Revirtiendo transacción.");
                transaction.Rollback();
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            _logger.LogInformation("Realizando borrado del cliente con Id: {ClientId}", id);
            // Solo necesitamos eliminar de la tabla 'user'. ON DELETE CASCADE se encargará del resto.
            const string sql = """
                DELETE FROM "user" WHERE id = @Id AND "role" = 'Client';
                """;

            using var conn = CreateConnection();
            var affectedRows = await conn.ExecuteAsync(sql, new { Id = id });

            return affectedRows > 0;
        }
    }
}