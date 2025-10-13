using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace GYMPT.Pages.Disciplines
{
    public class DisciplineEditModel : PageModel
    {
        private readonly IDisciplineService _disciplineService;
        private readonly IUserService _userService;

        [BindProperty]
        public Discipline Discipline { get; set; }
        public SelectList InstructorOptions { get; set; }

        public DisciplineEditModel(IDisciplineService disciplineService, IUserService userService)
        {
            _disciplineService = disciplineService;
            _userService = userService;
        }

        // Este método se ejecuta al cargar la página. Recibe un 'int' desde la URL.
        public async Task<IActionResult> OnGetAsync(int id)
        {
            // Llama al servicio para obtener los datos de la disciplina a editar.
            Discipline = await _disciplineService.GetDisciplineById(id);

            if (Discipline == null)
            {
                TempData["ErrorMessage"] = "Disciplina no encontrada.";
                return RedirectToPage("./Discipline");
            }

            // Prepara el menú desplegable de instructores.
            await PopulateInstructorsDropDownList();
            return Page();
        }

        // Este método se ejecuta al guardar el formulario.
        public async Task<IActionResult> OnPostAsync()
        {
            // Si los datos del formulario no son válidos, vuelve a mostrar el formulario con los errores.
            if (!ModelState.IsValid)
            {
                await PopulateInstructorsDropDownList();
                return Page();
            }

            // Llama al servicio para actualizar los datos.
            var success = await _disciplineService.UpdateDisciplineData(Discipline);

            if (success)
            {
                TempData["SuccessMessage"] = "Los datos de la disciplina han sido actualizados.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo actualizar la disciplina.";
            }

            // Redirige de vuelta a la lista principal.
            return RedirectToPage("./Discipline");
        }

        // Método auxiliar para no repetir código.
        private async Task PopulateInstructorsDropDownList()
        {
            var users = await _userService.GetAllUsers();
            var instructors = users
                .Where(u => u.Role == "Instructor") // Asumiendo que el rol se llama "Instructor"
                .Select(u => new { Id = (long)u.Id, FullName = $"{u.Name} {u.FirstLastname}" });

            // Crea el SelectList, pasando el IdInstructor actual para que aparezca seleccionado.
            InstructorOptions = new SelectList(instructors, "Id", "FullName", Discipline?.IdInstructor);
        }
    }
}