using GYMPT.Models;

namespace GYMPT.Data
{
    public class MembershipRepository : IMembershipRepository
    {
        private readonly Supabase.Client _supabase;

        public MembershipRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<IEnumerable<Membership>> GetAllAsync()
        {
            var response = await _supabase.From<Membership>().Get();
            return response.Models;
        }
    }
}