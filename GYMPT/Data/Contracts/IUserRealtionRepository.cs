namespace GYMPT.Data.Contracts
{
    public interface IUserRelationRepository<T>
    {
        Task<T> GetByIdAsync(int id);
        Task<bool> DeleteByIdAsync(int id);
    }
}