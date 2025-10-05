using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GYMPT.Models;

namespace GYMPT.Data
{
    public class GYMPTContext : DbContext
    {
        public GYMPTContext (DbContextOptions<GYMPTContext> options)
            : base(options)
        {
        }

        public DbSet<GYMPT.Models.Discipline> Discipline { get; set; } = default!;
    }
}
