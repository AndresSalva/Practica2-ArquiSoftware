using GYMPT.Data.Contracts;
using GYMPT.Models;
using GYMPT.Services;

namespace GYMPT.Data.Repositories
{
    public class UserRepository : IRepository<UserData>
    {
        private readonly Supabase.Client _supabase;

        public UserRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<UserData>> GetAllAsync()
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo("Se solicitó la lista completa de usuarios.");
                var response = await _supabase.From<UserData>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Fallo crítico al obtener la lista de usuarios.", ex);
                throw;
            }
        }
    }
}