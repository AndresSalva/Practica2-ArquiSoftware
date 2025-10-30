using GYMPT.Domain.Ports;
using ServiceUser.Domain.Entities;
using ServiceUser.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class LoginService
    {
        private readonly IUserRepository _instructorRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginService(IUserRepository instructorRepository, IPasswordHasher passwordHasher)
        {
            _instructorRepository = instructorRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<User> AuthenticateAsync(string email, string password)
        {
            var instructor = await _instructorRepository.GetByEmailAsync(email);
            if (instructor != null && _passwordHasher.Verify(instructor.Password, password))
            {
                return instructor;
            }
            return null;
        }
    }
}