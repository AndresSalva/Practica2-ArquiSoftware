using Dapper;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Services;
using Npgsql;
using ServiceUser.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Infrastructure.Persistence
{
    public class ClientRepository : IClientRepository
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
                    p.id AS Id,
                    p.name AS Name,
                    p.first_lastname AS FirstLastname,
                    p.second_lastname AS SecondLastname,
                    p.date_birth AS DateBirth,
                    p.ci AS Ci,
                    p.created_at AS CreatedAt,
                    p.last_modification AS LastModification,
                    p.is_active AS IsActive,
                    c.fitness_level AS FitnessLevel,
                    c.initial_weight_kg AS InitialWeightKg,
                    c.current_weight_kg AS CurrentWeightKg,
                    c.emergency_contact_phone AS EmergencyContactPhone
                FROM ""person"" p
                INNER JOIN client c ON p.id = c.id_person
                WHERE p.id = @Id AND p.is_active = true;";

            return await conn.QuerySingleOrDefaultAsync<Client>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Client>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string sql = @"
                SELECT 
                    p.id AS Id,
                    p.name AS Name,
                    p.first_lastname AS FirstLastname,
                    p.second_lastname AS SecondLastname,
                    p.date_birth AS DateBirth,
                    p.ci AS Ci,
                    p.created_at AS CreatedAt,
                    p.last_modification AS LastModification,
                    p.is_active AS IsActive,
                    c.fitness_level AS FitnessLevel,
                    c.initial_weight_kg AS InitialWeightKg,
                    c.current_weight_kg AS CurrentWeightKg,
                    c.emergency_contact_phone AS EmergencyContactPhone
                FROM ""person"" p
                INNER JOIN client c ON p.id = c.id_person
                WHERE p.is_active = true;";

            return await conn.QueryAsync<Client>(sql);
        }

        public async Task<Client> CreateAsync(Client entity)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                // Insert en tabla "person"
                var personSql = @"
                    INSERT INTO ""person"" 
                    (name, first_lastname, second_lastname, date_birth, ci, created_at, last_modification, is_active)
                    VALUES 
                    (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @CreatedAt, @LastModification, @IsActive)
                    RETURNING id;";

                var newPersonId = await conn.QuerySingleAsync<int>(personSql, entity, transaction);
                entity.Id = newPersonId;
                entity.IdUser = newPersonId;

                // Insert en tabla "client"
                var clientSql = @"
                    INSERT INTO client 
                    (id_person, fitness_level, initial_weight_kg, current_weight_kg, emergency_contact_phone) 
                    VALUES 
                    (@IdUser, @FitnessLevel, @InitialWeightKg, @CurrentWeightKg, @EmergencyContactPhone);";

                await conn.ExecuteAsync(clientSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
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

                // Actualizar tabla "person"
                var personSql = @"
                    UPDATE ""person"" 
                    SET name = @Name,
                        first_lastname = @FirstLastname,
                        second_lastname = @SecondLastname,
                        date_birth = @DateBirth,
                        ci = @Ci,
                        last_modification = @LastModification
                    WHERE id = @Id;";

                await conn.ExecuteAsync(personSql, entity, transaction);

                // Actualizar tabla "client"
                var clientSql = @"
                    UPDATE client 
                    SET fitness_level = @FitnessLevel,
                        initial_weight_kg = @InitialWeightKg,
                        current_weight_kg = @CurrentWeightKg,
                        emergency_contact_phone = @EmergencyContactPhone
                    WHERE id_person = @Id;";

                await conn.ExecuteAsync(clientSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            var sql = @"
                UPDATE ""person"" 
                SET is_active = false, 
                    last_modification = @LastModification 
                WHERE id = @Id;";

            var affectedRows = await conn.ExecuteAsync(sql, new { Id = id, LastModification = DateTime.UtcNow });
            return affectedRows > 0;
        }
    }
}
        