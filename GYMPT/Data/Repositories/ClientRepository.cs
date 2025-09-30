using GYMPT.Data.Contracts;
using GYMPT.Domain;
using GYMPT.Factories;
using GYMPT.Models; 
using GYMPT.Services;
using GYMPT.Mappers;
using System;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly Supabase.Client _supabase;

        public ClientRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Client> GetByIdAsync(long id)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo($"Iniciando ensamblaje de cliente con ID: {id}.");

                var userTask = _supabase.From<UserData>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Single();
                var detailsTask = _supabase.From<ClientData>().Filter("id_user", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Single();

                await Task.WhenAll(userTask, detailsTask);

                var baseUserData = userTask.Result;
                var details = detailsTask.Result;

                if (baseUserData == null || baseUserData.Role != "Client")
                {
                    _ = RemoteLoggerSingleton.Instance.LogWarning($"No se encontró un usuario base con ID: {id} y rol 'Client'.");
                    return null;
                }

                var client = UserMapper.MapToUserDomain<Client>(baseUserData);

                if (details != null)
                {
                    client.FitnessLevel = details.FitnessLevel;
                    client.InitialWeightKg = details.InitialWeightKg;
                    client.CurrentWeightKg = details.CurrentWeightKg;
                    client.EmergencyContactPhone = details.EmergencyContactPhone;
                }

                _ = RemoteLoggerSingleton.Instance.LogInfo($"Ensamblaje de cliente con ID: {id} completado.");
                return client;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Fallo al ensamblar cliente con ID: {id}.", ex);
                throw;
            }
        }
    }
}