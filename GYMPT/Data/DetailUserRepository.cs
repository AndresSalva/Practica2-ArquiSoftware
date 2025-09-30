using GYMPT.Models;
using GYMPT.Services;
using Supabase.Postgrest.Exceptions;

namespace GYMPT.Data
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly Supabase.Client _supabase;

        public DetailUserRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        // CREATE
        public async Task<DetailsUser?> CreateAsync(DetailsUser entity)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo("Creando un nuevo detalle de usuario...");
                var response = await _supabase.From<DetailsUser>().Insert(entity);
                return response.Models.FirstOrDefault();
            }
            catch (PostgrestException pgEx)
            {
                await RemoteLoggerSingleton.Instance.LogError("Error de Postgrest al crear detalle de usuario.", pgEx);
                throw;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError("Error inesperado al crear detalle de usuario.", ex);
                throw;
            }
        }

        // READ ALL
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

        // READ BY ID
        public async Task<DetailsUser?> GetByIdAsync(long id)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo($"Buscando detalle de usuario con ID {id}...");
                var response = await _supabase.From<DetailsUser>().Where(x => x.Id == id).Get();
                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al buscar detalle de usuario con ID {id}.", ex);
                throw;
            }
        }

        // UPDATE
        public async Task<DetailsUser?> UpdateAsync(DetailsUser entity)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo($"Actualizando detalle de usuario con ID {entity.Id}...");
                entity.LastModification = DateTime.UtcNow;
                var response = await _supabase.From<DetailsUser>().Update(entity);
                return response.Models.FirstOrDefault();
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al actualizar detalle de usuario con ID {entity.Id}.", ex);
                throw;
            }
        }

        // DELETE
        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo($"Eliminando detalle de usuario con ID {id}...");
                var response = await _supabase.From<DetailsUser>().Where(x => x.Id == id).Delete();
                return response.Models.Count > 0;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Error al eliminar detalle de usuario con ID {id}.", ex);
                throw;
            }
        }
    }
}
