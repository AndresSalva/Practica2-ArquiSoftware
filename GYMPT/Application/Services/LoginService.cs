using GYMPT.Domain.Ports;
using GYMPT.Domain.Entities;

namespace GYMPT.Application.Services
{
    public class LoginService
    {
        private readonly IInstructorRepository _instructorRepository;
        private readonly IPasswordHasher _passwordHasher;

        public LoginService(IInstructorRepository instructorRepository, IPasswordHasher passwordHasher)
        {
            _instructorRepository = instructorRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<Instructor> AuthenticateAsync(string email, string password)
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