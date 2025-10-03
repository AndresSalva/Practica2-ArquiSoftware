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
    public class UserRepository : IRepository<UserData>
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<IEnumerable<UserData>> GetAllAsync()
        {
            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de usuarios con Dapper.");
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT id, created_at AS CreatedAt, name, first_lastname AS FirstLastname, role FROM \"User\"";
                return await conn.QueryAsync<UserData>(sql);
            }
        }
    }
}