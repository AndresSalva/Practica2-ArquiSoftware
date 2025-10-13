using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;
using GYMPT.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Linq;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {
        [BindProperty]
        public Instructor Instructor { get; set; } = new Instructor();

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        private IRepository<User> CreateUserRepository() => new UserRepositoryCreator().CreateRepository();
        private IRepository<Instructor> CreateInstructorRepository() => new InstructorRepositoryCreator().CreateRepository();

        private void AddModelErrorIfFail(Result result, string key)
        {
            if (result.IsFailure)
                ModelState.AddModelError(key, result.Error);
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var instructorRepo = CreateInstructorRepository();
            Instructor = await instructorRepo.GetByIdAsync(Id);

            if (Instructor == null)
                return RedirectToPage("/Users/User");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // -------------------------
            // VALIDACIONES DE USUARIO
            // -------------------------
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Instructor.Name), "Instructor.Name");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Instructor.FirstLastname), "Instructor.FirstLastname");
            AddModelErrorIfFail(UserRules.NombreCompletoValido(Instructor.SecondLastname), "Instructor.SecondLastname");
            AddModelErrorIfFail(UserRules.CiValido(Instructor.Ci), "Instructor.Ci");
            AddModelErrorIfFail(UserRules.FechaNacimientoValida(Instructor.DateBirth), "Instructor.DateBirth");

            // -------------------------
            // VALIDACIONES DE DOMINIO
            // -------------------------
            AddModelErrorIfFail(InstructorRules.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth), "Instructor.HireDate");
            AddModelErrorIfFail(InstructorRules.EsEspecializacionValida(Instructor.Specialization), "Instructor.Specialization");
            AddModelErrorIfFail(InstructorRules.EsSalarioValido(Instructor.MonthlySalary), "Instructor.MonthlySalary");

            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // ACTUALIZAR USER E INSTRUCTOR
            // -------------------------
            var userRepo = CreateUserRepository();
            var instructorRepo = CreateInstructorRepository();

            var existingUser = (await userRepo.GetAllAsync()).FirstOrDefault(u => u.Ci == Instructor.Ci);

            if (existingUser == null)
            {
                // Esto no debería pasar normalmente en edición, pero se puede dejar por seguridad
                var newUser = new User
                {
                    Name = Instructor.Name,
                    FirstLastname = Instructor.FirstLastname,
                    SecondLastname = Instructor.SecondLastname,
                    Ci = Instructor.Ci,
                    DateBirth = Instructor.DateBirth,
                    Role = "Instructor",
                    IsActive = true
                };

                await userRepo.CreateAsync(newUser);
                Instructor.IdUser = newUser.Id;
            }
            else
            {
                Instructor.IdUser = existingUser.Id;
                // Actualizamos datos básicos del user
                existingUser.Name = Instructor.Name;
                existingUser.FirstLastname = Instructor.FirstLastname;
                existingUser.SecondLastname = Instructor.SecondLastname;
                existingUser.DateBirth = Instructor.DateBirth;
                await userRepo.UpdateAsync(existingUser);
            }

            await instructorRepo.UpdateAsync(Instructor);

            TempData["Message"] = $"Instructor '{Instructor.Name}' actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
