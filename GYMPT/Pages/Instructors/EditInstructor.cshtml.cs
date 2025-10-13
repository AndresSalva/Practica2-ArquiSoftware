using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using GYMPT.Domain.Rules;
using GYMPT.Infrastructure.Factories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace GYMPT.Pages.Instructors
{
    public class EditInstructorModel : PageModel
    {
        [BindProperty]
        public Instructor Instructor { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Id { get; set; }

        private IRepository<Instructor> CreateInstructorRepository() => new InstructorRepositoryCreator().CreateRepository();

        public async Task<IActionResult> OnGetAsync()
        {
            Instructor = await CreateInstructorRepository().GetByIdAsync(Id);
            if (Instructor == null) return RedirectToPage("/Users/User");
            Id = Instructor.Id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // -------------------------
            // Validaciones de Usuario + Instructor
            // -------------------------
            if (!UserRules.NombreCompletoValido(Instructor.Name))
                ModelState.AddModelError("Instructor.Name", "Nombre inválido.");

            if (!UserRules.NombreCompletoValido(Instructor.FirstLastname))
                ModelState.AddModelError("Instructor.FirstLastname", "Apellido Paterno inválido.");

            if (!UserRules.NombreCompletoValido(Instructor.SecondLastname))
                ModelState.AddModelError("Instructor.SecondLastname", "Apellido Materno inválido.");

            if (!UserRules.CiValido(Instructor.Ci))
                ModelState.AddModelError("Instructor.Ci", "CI inválido.");

            if (!UserRules.FechaNacimientoValida(Instructor.DateBirth))
                ModelState.AddModelError("Instructor.DateBirth", "Fecha de nacimiento inválida.");

            if (!InstructorRules.EsFechaContratacionValida(Instructor.HireDate, Instructor.DateBirth))
                ModelState.AddModelError("Instructor.HireDate", "Fecha de contratación inválida.");

            if (!InstructorRules.EsEspecializacionValida(Instructor.Specialization))
                ModelState.AddModelError("Instructor.Specialization", "Especialización inválida.");

            if (!InstructorRules.EsSalarioValido(Instructor.MonthlySalary))
                ModelState.AddModelError("Instructor.MonthlySalary", "Salario inválido.");

            if (!ModelState.IsValid) return Page();

            Instructor.Id = Id;
            await CreateInstructorRepository().UpdateAsync(Instructor);
            TempData["Message"] = $"Instructor {Instructor.Name} actualizado correctamente.";
            return RedirectToPage("/Users/User");
        }
    }
}
