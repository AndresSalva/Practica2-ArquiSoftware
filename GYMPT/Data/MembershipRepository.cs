using GYMPT.Models;
using GYMPT.Services;

namespace GYMPT.Data
{
    public class MembershipRepository : IRepository<Membership>
    {
        private readonly Supabase.Client _supabase;

        public MembershipRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo("Se solicitó la lista completa de membresías.");
                var response = await _supabase.From<Membership>().Get();
                return response.Models;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Fallo crítico al obtener la lista de membresías.", ex);
                throw;
            }
        }
    }
}