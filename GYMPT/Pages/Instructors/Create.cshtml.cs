using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;
using GYMPT.Domain.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Name { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FirstLastname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SecondLastname { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Ci { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime DateBirth { get; set; }

        [BindProperty]
        public Instructor Instructor { get; set; } = new Instructor();

        private IRepository<User> CreateUserRepository() => new UserRepositoryCreator().CreateRepository();
        private IRepository<Instructor> CreateInstructorRepository() => new InstructorRepositoryCreator().CreateRepository();

        // -------------------------
        // MÉTODO AUXILIAR PARA RESULT
        // -------------------------
        private void AddModelErrorIfFail(Result result, string key)
        {
            if (result.IsFailure)
                ModelState.AddModelError(key, result.Error);
        }

        public void OnGet()
        {
            // Prellenar datos básicos del usuario
            Instructor.Name = Name;
            Instructor.FirstLastname = FirstLastname;
            Instructor.SecondLastname = SecondLastname;
            Instructor.Ci = Ci;
            Instructor.DateBirth = DateBirth;
            Instructor.Role = "Instructor";
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
            // VALIDACIONES DE DOMINIO INSTRUCTOR
            // -------------------------
            AddModelErrorIfFail(InstructorRules.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth), "Instructor.HireDate");
            AddModelErrorIfFail(InstructorRules.EsEspecializacionValida(Instructor.Specialization), "Instructor.Specialization");
            AddModelErrorIfFail(InstructorRules.EsSalarioValido(Instructor.MonthlySalary), "Instructor.MonthlySalary");

            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // COMPLETAR DATOS BÁSICOS SI VIENEN VACÍOS
            // -------------------------
            Instructor.Name = string.IsNullOrWhiteSpace(Instructor.Name) ? Name : Instructor.Name;
            Instructor.FirstLastname = string.IsNullOrWhiteSpace(Instructor.FirstLastname) ? FirstLastname : Instructor.FirstLastname;
            Instructor.SecondLastname = string.IsNullOrWhiteSpace(Instructor.SecondLastname) ? SecondLastname : Instructor.SecondLastname;
            Instructor.Ci = string.IsNullOrWhiteSpace(Instructor.Ci) ? Ci : Instructor.Ci;
            Instructor.DateBirth = Instructor.DateBirth == default ? DateBirth : Instructor.DateBirth;

            Instructor.Role = "Instructor";
            Instructor.CreatedAt = DateTime.UtcNow;
            Instructor.LastModification = DateTime.UtcNow;
            Instructor.IsActive = true;

            // -------------------------
            // GUARDAR USER E INSTRUCTOR
            // -------------------------
            var userRepo = CreateUserRepository();
            var instructorRepo = CreateInstructorRepository();

            try
            {
                var existingUser = (await userRepo.GetAllAsync()).FirstOrDefault(u => u.Ci == Instructor.Ci);

                if (existingUser == null)
                {
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
                }

                await instructorRepo.CreateAsync(Instructor);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error al guardar instructor: {ex.Message}");
                return Page();
            }

            TempData["Message"] = $"Instructor '{Instructor.Name}' creado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
