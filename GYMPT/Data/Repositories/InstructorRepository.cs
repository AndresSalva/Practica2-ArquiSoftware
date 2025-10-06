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
                var userSql = "SELECT id, created_at AS CreatedAt, name, first_lastname AS FirstLastname, second_lastname AS SecondLastname, date_birth as DateBirth, \"CI\", role FROM \"User\" WHERE id = @Id";
                var baseUser = await conn.QuerySingleOrDefaultAsync<User>(userSql, new { Id = id });

                if (baseUser == null || baseUser.Role != "Instructor")
                {
                    await RemoteLoggerSingleton.Instance.LogWarning($"No se encontró un usuario base con ID: {id} y rol 'Instructor'.");
                    return null;
                }

                var instructor = UserMapper.MapToUserDomain<Instructor>(baseUser);

                var detailsSql = "SELECT hire_date AS HireDate, monthly_salary AS MonthlySalary, specialization AS Specialization FROM \"Instructor\" WHERE id_user = @Id";
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
                        var userSql = "INSERT INTO \"User\" (name, first_lastname, second_lastname, date_birth, \"CI\", role, \"isActive\") VALUES (@Name, @FirstLastname, @SecondLastname, @DateBirth, @CI, @Role, true) RETURNING id;";
                        var newUserId = await conn.QuerySingleAsync<int>(userSql, instructor, transaction);

                        instructor.Id = newUserId;
                        var instructorSql = "INSERT INTO \"Instructor\" (id_user, hire_date, monthly_salary, specialization) VALUES (@Id, @HireDate, @MonthlySalary, @Specialization);";
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

        public Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}