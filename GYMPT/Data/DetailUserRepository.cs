using GYMPT.Models;
using GYMPT.Services;

namespace GYMPT.Data
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly Supabase.Client _supabase;

        public DetailUserRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo("Se solicitó la lista completa de detalles de usuarios.");
                var response = await _supabase.From<DetailsUser>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Fallo crítico al obtener la lista de detalles de usuarios.", ex);
                throw;
            }
        }
    }
}