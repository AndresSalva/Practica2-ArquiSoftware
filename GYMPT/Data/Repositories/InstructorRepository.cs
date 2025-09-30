using GYMPT.Data.Contracts;
using GYMPT.Domain;
using GYMPT.Factories;
using GYMPT.Models;
using GYMPT.Services;
using System;
using System.Threading.Tasks;

namespace GYMPT.Data.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        private readonly Supabase.Client _supabase;

        public InstructorRepository(Supabase.Client supabase)
        {
            _supabase = supabase;
        }

        public async Task<Instructor> GetByIdAsync(long id)
        {
            try
            {
                _ = RemoteLoggerSingleton.Instance.LogInfo($"Iniciando ensamblaje de instructor con ID: {id}.");

                var userTask = _supabase.From<UserData>().Filter("id", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Single();
                var detailsTask = _supabase.From<InstructorData>().Filter("id_user", Supabase.Postgrest.Constants.Operator.Equals, id.ToString()).Single();
                await Task.WhenAll(userTask, detailsTask);

                var baseUserData = userTask.Result;
                var details = detailsTask.Result;

                if (baseUserData == null || baseUserData.Role != "Instructor")
                {
                    _ = RemoteLoggerSingleton.Instance.LogWarning($"No se encontró un usuario base con ID: {id} y rol 'Instructor'.");
                    return null;
                }

                var instructor = (Instructor)new InstructorCreator().CreateUser();

                MapBaseUserData(baseUserData, instructor); 

                if (details != null)
                {
                    instructor.HireDate = details.HireDate;
                    instructor.MonthlySalary = details.MonthlySalary;
                    instructor.Specialization = details.Specialization;
                }

                _ = RemoteLoggerSingleton.Instance.LogInfo($"Ensamblaje de instructor con ID: {id} completado.");
                return instructor;
            }
            catch (Exception ex)
            {
                await RemoteLoggerSingleton.Instance.LogError($"Fallo al ensamblar instructor con ID: {id}.", ex);
                throw;
            }
        }

        private void MapBaseUserData(UserData source, User destination)
        {
            destination.Id = source.Id;
            destination.CreatedAt = source.CreatedAt;
            destination.LastModification = source.LastModification;
            destination.IsActive = source.IsActive;
            destination.Name = source.Name;
            destination.FirstLastname = source.FirstLastname;
            destination.SecondLastname = source.SecondLastname;
            destination.DateBirth = source.DateBirth;
            destination.CI = source.CI;
            destination.Role = source.Role;
        }
    }
}