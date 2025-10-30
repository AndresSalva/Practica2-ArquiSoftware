using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class LoginService
    {
        private readonly IUserRepository _instructorRepository;

        public LoginService(IUserRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var instructor = await _instructorRepository.GetByEmailAsync(email);
            if (instructor != null)
            {
                return instructor;
            }
            return null;
        }
    }
}