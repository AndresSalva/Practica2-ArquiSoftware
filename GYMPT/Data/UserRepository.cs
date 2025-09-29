using GYMPT.Models;
using GYMPT.Services;

namespace GYMPT.Data
{
    public class UserRepository : IMembershipRepository<User>
    {
        private readonly Supabase.Client _supabase;

        public UserRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo("Se solicitó la lista completa de disciplinas.");
                var response = await _supabase.From<User>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Fallo crítico al obtener la lista de disciplinas.", ex);
                throw;
            }
        }
    }
}