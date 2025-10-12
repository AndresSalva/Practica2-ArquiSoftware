using Dapper;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Infrastructure.Services;
using Npgsql;

namespace GYMPT.Infrastructure.Persistence
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly string _postgresString;

        public InstructorRepository()
        {
            _postgresString = ConnectionStringSingleton.Instance.PostgresConnection;
        }

        public async Task<Instructor> GetByIdAsync(int id)
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
                    i.hire_date AS HireDate,
                    i.monthly_salary AS MonthlySalary,
                    i.specialization AS Specialization
                FROM ""user"" u
                INNER JOIN instructor i ON u.id = i.id_user
                WHERE u.id = @Id AND u.is_active = true;";

            return await conn.QuerySingleOrDefaultAsync<Instructor>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
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
                    i.hire_date AS HireDate,
                    i.monthly_salary AS MonthlySalary,
                    i.specialization AS Specialization
                FROM ""user"" u
                INNER JOIN instructor i ON u.id = i.id_user
                WHERE u.is_active = true AND u.role = 'Instructor';";

            return await conn.QueryAsync<Instructor>(sql);
        }

        public async Task<Instructor> CreateAsync(Instructor entity)
        {

            await RemoteLoggerSingleton.Instance.LogInfo($"Creating instructor: {entity.Name}");
            using var conn = new NpgsqlConnection(_postgresString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                entity.Role = "Instructor";
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModification = DateTime.UtcNow;
                entity.IsActive = true;

                var userSql = @"INSERT INTO ""user"" (name, first_lastname, second_lastname, date_birth, ci, role, created_at, last_modification, is_active) VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role, @CreatedAt, @LastModification, @IsActive) RETURNING id;";
                var newUserId = await conn.QuerySingleAsync<int>(userSql, entity, transaction);
                entity.Id = newUserId;
                entity.IdUser = newUserId;

                var instructorSql = @"INSERT INTO instructor (id_user, hire_date, monthly_salary, specialization) VALUES (@IdUser, @HireDate, @MonthlySalary, @Specialization);";
                await conn.ExecuteAsync(instructorSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to create instructor {entity.Name}.", ex);
                throw;
            }
        }

        public async Task<Instructor> UpdateAsync(Instructor entity)
        {

            using var conn = new NpgsqlConnection(_postgresString);
            await conn.OpenAsync();
            using var transaction = await conn.BeginTransactionAsync();
            try
            {
                entity.LastModification = DateTime.UtcNow;
                var userSql = @"UPDATE ""user"" SET name = @Name, first_lastname = @FirstLastname, second_lastname = @SecondLastname, date_birth = @DateBirth, ci = @Ci, last_modification = @LastModification WHERE id = @Id;";
                await conn.ExecuteAsync(userSql, entity, transaction);

                var instructorSql = @"UPDATE instructor SET hire_date = @HireDate, monthly_salary = @MonthlySalary, specialization = @Specialization WHERE id_user = @Id;";
                await conn.ExecuteAsync(instructorSql, entity, transaction);

                await transaction.CommitAsync();
                return entity;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                await RemoteLoggerSingleton.Instance.LogError($"Error trying to update instructor {entity.Id}.", ex);
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