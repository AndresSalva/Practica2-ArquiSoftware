namespace GYMPT.Application.Interfaces
{
    public interface IUserContextService
    {
        int? GetUserId();
        string? GetUserRole();
        string? GetUserName();
    }
}
