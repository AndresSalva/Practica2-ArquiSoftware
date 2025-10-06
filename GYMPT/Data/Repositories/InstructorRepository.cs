using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Mappers;
using GYMPT.Models;
using GYMPT.Services;
using Npgsql;

namespace GYMPT.Data.Repositories
{
    public class InstructorRepository : IUserRelationRepository<Instructor>
    {
        private readonly string _connectionString;

        public InstructorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<Instructor> GetByIdAsync(int id)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Buscando instructor con ID: {id} en PostgreSQL con Dapper.");

            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var userSql =
                @"SELECT id, created_at AS CreatedAt, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname,
                date_birth as DateBirth, ci, role 
                FROM ""user"" WHERE id = @Id AND is_active = true;";
                var baseUser = await conn.QuerySingleOrDefaultAsync<User>(userSql, new { Id = id });

                if (baseUser == null || baseUser.Role != "Instructor")
                {
                    await RemoteLoggerSingleton.Instance.LogWarning($"No se encontró un usuario base con ID: {id} y rol 'Instructor'.");
                    return null;
                }

                var instructor = UserMapper.MapToUserDomain<Instructor>(baseUser);

                var detailsSql = "SELECT hire_date AS HireDate, monthly_salary AS MonthlySalary, specialization AS Specialization FROM instructor WHERE id_user = @Id";
                var detailsData = await conn.QuerySingleOrDefaultAsync<Instructor>(detailsSql, new { Id = id });

                if (detailsData != null)
                {
                    instructor.HireDate = detailsData.HireDate;
                    instructor.MonthlySalary = detailsData.MonthlySalary;
                    instructor.Specialization = detailsData.Specialization;
                }

                await RemoteLoggerSingleton.Instance.LogInfo($"Ensamblaje de instructor con ID: {id} completado.");
                return instructor;
            }
        }

        public async Task CreateAsync(Instructor instructor)
        {
            await RemoteLoggerSingleton.Instance.LogInfo($"Iniciando creación de un nuevo instructor: {instructor.Name} con Dapper.");
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
                        VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @Ci, @Role)
                        RETURNING id;";
                        var newUserId = await conn.QuerySingleAsync<int>(userSql, instructor, transaction);

                        instructor.Id = newUserId;
                        var instructorSql =
                        @"INSERT INTO instructor
                        (id_user, hire_date, monthly_salary, specialization)
                        VALUES (@Id, @HireDate, @MonthlySalary, @Specialization);";
                        await conn.ExecuteAsync(instructorSql, instructor, transaction);

                        await transaction.CommitAsync();
                        await RemoteLoggerSingleton.Instance.LogInfo($"Instructor con ID {newUserId} creado exitosamente.");
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        await RemoteLoggerSingleton.Instance.LogError($"Error al crear instructor {instructor.Name}.", ex);
                        throw;
                    }
                }
            }
        }

        public async Task<bool> UpdateAsync(Instructor instructor)
        {
            using var conn = new NpgsqlConnection(_connectionString);

            var sql =
            @"UPDATE instructor
            SET specialization = @Specialization,
            hire_date = @HireDate,
            monthly_salary = @MonthlySalary
            WHERE id_user = @Id;";

            var parameters = new
            {
                instructor.Id,
                LastModification = DateTime.Now,
                instructor.Specialization,
                instructor.HireDate,
                instructor.MonthlySalary
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