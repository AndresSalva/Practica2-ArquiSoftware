using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GYMPT.Data;
using GYMPT.Models;

namespace GYMPT.Pages
{
    public class DetailsModel : PageModel
    {
        private readonly GYMPT.Data.GYMPTContext _context;

        public DetailsModel(GYMPT.Data.GYMPTContext context)
        {
            _context = context;
        }

        public Discipline Discipline { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var discipline = await _context.Discipline.FirstOrDefaultAsync(m => m.Id == id);
            if (discipline == null)
            {
                return NotFound();
            }
            else
            {
                Discipline = discipline;
            }
            return Page();
        }
    }
}
