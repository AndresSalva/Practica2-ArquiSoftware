namespace ServiceCommon.Domain.Ports
{
    public interface IRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<bool> DeleteByIdAsync(int id);
    }
}