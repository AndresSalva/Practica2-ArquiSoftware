using GYMPT.Domain.Entities;
using ServiceUser.Domain.Ports;
using ServiceUser.Domain.Entities;

namespace GYMPT.Domain.Ports
{
    public interface IUserRepository : IRepository<User>
    {
    }
}