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
                SELECT * FROM instructor_view
                WHERE Id = @Id;";

            return await conn.QuerySingleOrDefaultAsync<Instructor>(sql, new { Id = id });
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string sql = @"
                SELECT * FROM instructor_view   
                WHERE Role = 'Instructor';";

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

                var instructorSql = @"INSERT INTO instructor (id_user, hire_date, monthly_salary, specialization,email,password) VALUES (@IdUser, @HireDate, @MonthlySalary, @Specialization,@Email,@Password);";
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

                var instructorSql = @"UPDATE instructor SET hire_date = @HireDate, monthly_salary = @MonthlySalary, specialization = @Specialization,email = @Email WHERE id_user = @Id;";
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

        public async Task<Instructor> GetByEmailAsync(string email)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string sql = @"SELECT * FROM instructor_view WHERE email = @Email;";

            return await conn.QuerySingleOrDefaultAsync<Instructor>(sql, new { Email = email });
        }

        public async Task<bool> UpdatePasswordAsync(int id, string password)
        {
            using var conn = new NpgsqlConnection(_postgresString);
            const string query = @"UPDATE instructor 
                       SET password = @Password, must_change_password = false 
                       WHERE id_user = @Id;";
            var rowsChanged = await conn.ExecuteAsync(query, new { Password = password, Id = id });
            return rowsChanged > 0;
        }
    }
}