using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;
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

        public void OnGet()
        {
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
            // VALIDACIONES DEL USUARIO
            // -------------------------
            if (!UserRules.NombreCompletoValido(Instructor.Name))
                ModelState.AddModelError("Instructor.Name", "Nombre inválido. Debe tener al menos 2 letras y solo letras y espacios.");

            if (!UserRules.NombreCompletoValido(Instructor.FirstLastname))
                ModelState.AddModelError("Instructor.FirstLastname", "Apellido Paterno inválido.");

            if (!UserRules.NombreCompletoValido(Instructor.SecondLastname))
                ModelState.AddModelError("Instructor.SecondLastname", "Apellido Materno inválido.");

            if (!UserRules.CiValido(Instructor.Ci))
                ModelState.AddModelError("Instructor.Ci", "CI inválido. Solo números y letras.");

            if (!UserRules.FechaNacimientoValida(Instructor.DateBirth))
                ModelState.AddModelError("Instructor.DateBirth", "Fecha de nacimiento no puede ser futura.");

            // -------------------------
            // VALIDACIONES DE DOMINIO DE INSTRUCTOR
            // -------------------------
            if (!InstructorRules.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth))
                ModelState.AddModelError("Instructor.HireDate",
                    "La fecha de contratación debe ser posterior al nacimiento, no puede estar en el futuro y el instructor debe tener al menos 18 años.");

            if (!InstructorRules.EsEspecializacionValida(Instructor.Specialization))
                ModelState.AddModelError("Instructor.Specialization", "Especialización inválida (mínimo 3 caracteres).");

            if (!InstructorRules.EsSalarioValido(Instructor.MonthlySalary))
                ModelState.AddModelError("Instructor.MonthlySalary", "Salario inválido (debe ser ≥ 0).");

            if (!ModelState.IsValid) return Page();

            // -------------------------
            // GUARDAR USER E INSTRUCTOR
            // -------------------------
            var userRepo = CreateUserRepository();
            var instructorRepo = CreateInstructorRepository();

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

            return RedirectToPage("/Users/User");
        }
    }
}
