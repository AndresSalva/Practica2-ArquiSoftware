using GYMPT.Data.Contracts;
using GYMPT.Data.Repositories;
using GYMPT.Factories;
using GYMPT.Models;
using GYMPT.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {

        public EditInstructorModel()
        {
        }
        private IRepository<Instructor> CreateInstructorRepository()
        {
            var factory = new InstructorRepositoryCreator();
            return factory.CreateRepository();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var instructorRepo = CreateInstructorRepository();
            Instructor = await instructorRepo.GetByIdAsync(Id);
            if (Instructor == null) return RedirectToPage("/Users/User");
            Id = Instructor.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Instructor == null || Id == 0) return RedirectToPage("/Users/User");

            // -------------------------
            // VALIDACIONES
            // -------------------------
            if (!ValidacionesUsuario.EsNombreCompletoValido(Instructor.Name))
                ModelState.AddModelError("Instructor.Name", "Nombre inválido. Debe contener solo letras y mínimo 2 caracteres.");

            if (!ValidacionesUsuario.EsNombreCompletoValido(Instructor.FirstLastname))
                ModelState.AddModelError("Instructor.FirstLastname", "Apellido Paterno inválido.");

            if (!ValidacionesUsuario.EsNombreCompletoValido(Instructor.SecondLastname))
                ModelState.AddModelError("Instructor.SecondLastname", "Apellido Materno inválido.");

            if (!ValidacionesUsuario.EsCiValido(Instructor.Ci))
                ModelState.AddModelError("Instructor.Ci", "CI inválido. Solo números y letras.");

            if (!ValidacionesUsuario.EsFechaNacimientoValida(Instructor.DateBirth))
                ModelState.AddModelError("Instructor.DateBirth", "Fecha de nacimiento no puede ser futura.");

            if (!ValidacionesUsuario.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth))
                ModelState.AddModelError("Instructor.HireDate",
                    "La fecha de contratación debe ser posterior al nacimiento, no puede estar en el futuro y el instructor debe tener al menos 18 años.");

            if (!ValidacionesUsuario.EsEspecializacionValida(Instructor.Specialization))
                ModelState.AddModelError("Instructor.Specialization", "Especialización inválida (mínimo 3 caracteres).");

            if (!ValidacionesUsuario.EsSalarioValido(Instructor.MonthlySalary))
                ModelState.AddModelError("Instructor.MonthlySalary", "Salario mensual inválido (debe ser mayor o igual a 0).");

            if (!ModelState.IsValid)
                return Page();

            // -------------------------
            // Guardar cambios
            // -------------------------
            Instructor.Id = Id;
            var instructorRepo = CreateInstructorRepository();
            await instructorRepo.UpdateAsync(Instructor);

            TempData["Message"] = $"Instructor {Instructor.Name} actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }

    }
}
