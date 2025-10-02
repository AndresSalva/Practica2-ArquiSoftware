using Dapper;
using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class DisciplineRepository : IRepository<Discipline>
    {
        private readonly string _connectionString;

        public DisciplineRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Task<Discipline> CreateAsync(Discipline entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de disciplinas con Dapper.");
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT id, name, id_instructor AS IdInstructor, start_time AS StartTime, end_time AS EndTime, created_at AS CreatedAt, last_modification AS LastModification, \"isActive\" as IsActive FROM \"Discipline\"";
                return await conn.QueryAsync<Discipline>(sql);
            }
        }

        public Task<Discipline> UpdateAsync(Discipline entity)
        {
            throw new NotImplementedException();
        }
    }
}