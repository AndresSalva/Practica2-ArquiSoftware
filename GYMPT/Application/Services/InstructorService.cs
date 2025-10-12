using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;

namespace GYMPT.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;

        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public Task<Instructor> GetInstructorById(int id) => _instructorRepository.GetByIdAsync(id);
        public Task<IEnumerable<Instructor>> GetAllInstructors() => _instructorRepository.GetAllAsync();
        public Task CreateNewInstructor(Instructor newInstructor) => _instructorRepository.CreateAsync(newInstructor);
        public Task UpdateInstructorData(Instructor instructorToUpdate) => _instructorRepository.UpdateAsync(instructorToUpdate);
    }
}