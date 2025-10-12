using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using GYMPT.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class CreateModel : PageModel
    {
        // -------------------------
        // Datos que vienen del formulario User
        // -------------------------
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

        // -------------------------
        // Datos específicos del Instructor
        // -------------------------
        [BindProperty]
        public Instructor Instructor { get; set; } = new Instructor();

        private IRepository<User> CreateUserRepository()
        {
            var factory = new UserRepositoryCreator();
            return factory.CreateRepository();
        }

        private IRepository<Instructor> CreateInstructorRepository()
        {
            var factory = new InstructorRepositoryCreator();
            return factory.CreateRepository();
        }

        public void OnGet()
        {
            // Prellenar datos de Instructor desde los datos recibidos del formulario de User
            Instructor.Name = Name;
            Instructor.FirstLastname = FirstLastname;
            Instructor.SecondLastname = SecondLastname;
            Instructor.Ci = Ci;
            Instructor.DateBirth = DateBirth;
            Instructor.Role = "Instructor";
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // VALIDACIONES
            if (!ValidacionesUsuario.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth))
            {
                ModelState.AddModelError("Instructor.HireDate",
                    "La fecha de contratación debe ser posterior al nacimiento, no puede estar en el futuro y el instructor debe tener al menos 18 años.");
            }

            if (!ValidacionesUsuario.EsSalarioValido(Instructor.MonthlySalary))
                ModelState.AddModelError("Instructor.MonthlySalary", "Salario mensual inválido.");

            if (!ValidacionesUsuario.EsEspecializacionValida(Instructor.Specialization))
                ModelState.AddModelError("Instructor.Specialization", "Especialización inválida.");

            if (!ModelState.IsValid)
                return Page();

            var userRepo = CreateUserRepository();
            var instructorRepo = CreateInstructorRepository();

            // Buscar si ya existe un User con el mismo CI
            var existingUsers = await userRepo.GetAllAsync();
            var existingUser = existingUsers.FirstOrDefault(u => u.Ci == Instructor.Ci);

            if (existingUser == null)
            {
                // Crear nuevo User si no existe
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
                // Reutilizar el User existente
                Instructor.IdUser = existingUser.Id;
            }
            await instructorRepo.CreateAsync(Instructor);

            return RedirectToPage("/Users/User");
        }

    }
}
