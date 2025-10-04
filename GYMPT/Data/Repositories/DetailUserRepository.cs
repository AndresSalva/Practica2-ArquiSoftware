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
//    public class DetailUserRepository : IRepository<DetailsUser>
//    {
//        private readonly string _connectionString;

//        public DetailUserRepository(IConfiguration configuration)
//        {
//            _connectionString = configuration.GetConnectionString("DefaultConnection");
//        }

//        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
//        {
//            await RemoteLoggerSingleton.Instance.LogInfo("Solicitando la lista completa de detalles de usuarios con Dapper.");
//            using (var conn = new NpgsqlConnection(_connectionString))
//            {
//                var sql = "SELECT id, id_user AS IdUser, id_membership AS IdMembership, start_date AS StartDate, end_date AS EndDate, sessions_left AS SessionsLeft, created_at AS CreatedAt, last_modification AS LastModification, \"isActive\" as IsActive FROM \"Details_user\"";
//                return await conn.QueryAsync<DetailsUser>(sql);
//            }
//        }
//    }
//}