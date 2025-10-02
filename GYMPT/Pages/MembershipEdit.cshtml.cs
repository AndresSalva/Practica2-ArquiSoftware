using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration; 
using Npgsql;                             
using System.Threading.Tasks;

namespace GYMPT.Pages
{
    public class MembershipEditModel : PageModel
    {
        private readonly IRepository<Membership> _repo;

        private readonly IConfiguration _configuration;

        [BindProperty]
        public Membership Membership { get; set; }

        public MembershipEditModel(IRepository<Membership> repo, IConfiguration configuration)
        {
            _repo = repo;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            Membership = new Membership();

            await using (var conn = new NpgsqlConnection(connectionString))
            {
                string sql = @"SELECT id, name, price, description, monthly_sessions, ""isActive"" 
                               FROM ""Membership"" WHERE id = @Id";

                await using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    await conn.OpenAsync();

                    await using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            Membership.Id = reader.GetInt32(reader.GetOrdinal("id"));
                            Membership.Name = reader.GetString(reader.GetOrdinal("name"));
                            Membership.Price = reader.GetFloat(reader.GetOrdinal("price"));
                            Membership.Description = reader.GetString(reader.GetOrdinal("description"));
                            Membership.MonthlySessions = reader.GetInt16(reader.GetOrdinal("monthly_sessions"));
                        }
                        else
                        {
                            return NotFound();
                        }
                    }
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _repo.UpdateAsync(Membership);

            return RedirectToPage("./Memberships");
        }
    }
}