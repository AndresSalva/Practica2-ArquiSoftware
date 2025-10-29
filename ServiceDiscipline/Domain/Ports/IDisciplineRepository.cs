using ServiceDiscipline.Domain.Entities;

namespace ServiceDiscipline.Domain.Ports
{
    public interface IDisciplineRepository 
    {
        Task<IEnumerable<Discipline>> GetAllAsync();
        Task<Discipline> GetByIdAsync(int id);
        Task<Discipline> CreateAsync(Discipline entity);
        Task<Discipline> UpdateAsync(Discipline entity);
        Task<bool> DeleteByIdAsync(int id);
    }
}