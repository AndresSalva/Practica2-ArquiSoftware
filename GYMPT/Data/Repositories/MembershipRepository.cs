//using Dapper;
//using GYMPT.Data.Contracts;
//using GYMPT.Models;
//using GYMPT.Services;
//using Microsoft.Extensions.Configuration;
//using Npgsql;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace GYMPT.Data.Repositories
//{
//    public class MembershipRepository : IRepository<Membership>
//    {
//        private readonly string _connectionString;

//        public MembershipRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<IEnumerable<Membership>> GetAllAsync()
//        {
//            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de membresías con Dapper.");
//            using (var conn = new NpgsqlConnection(_connectionString))
//            {
//                var sql = "SELECT id, name, price, description, monthly_sessions AS MonthlySessions, created_at AS CreatedAt, last_modification AS LastModification, \"isActive\" as IsActive FROM \"Membership\"";
//                return await conn.QueryAsync<Membership>(sql);
//            }
//        }
//    }
//}