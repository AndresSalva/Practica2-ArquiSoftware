using GYMPT.Data.Contracts;
using GYMPT.Models;
using Microsoft.EntityFrameworkCore;

namespace GYMPT.Data
{
    public class DetailUserRepository : IRepository<DetailsUser>
    {
        private readonly AppDbContext _context;

        public DetailUserRepository(AppDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<DetailsUser> CreateAsync(DetailsUser entity)
        {
            entity.CreatedAt = DateTime.UtcNow;
            entity.IsActive = true;

            _context.DetailsUsers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // READ ALL
        public async Task<IEnumerable<DetailsUser>> GetAllAsync()
        {
            return await _context.DetailsUsers
                                 .Where(d => d.IsActive == true)
                                 .ToListAsync();
        }

        // UPDATE
        public async Task<DetailsUser> UpdateAsync(DetailsUser entity)
        {
            var existing = await _context.DetailsUsers.FindAsync(entity.Id);
            if (existing == null) return null;

            existing.IdMembership = entity.IdMembership;
            existing.StartDate = entity.StartDate;
            existing.EndDate = entity.EndDate;
            existing.SessionsLeft = entity.SessionsLeft;
            existing.LastModification = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        // DELETE (lógico → marca como inactivo)
        public async Task<bool> DeleteByIdAsync(long id)
        {
            var existing = await _context.DetailsUsers.FindAsync(id);
            if (existing == null) return false;

            existing.IsActive = false;
            existing.LastModification = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}