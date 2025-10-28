using GYMPT.Application.Interfaces;
using GYMPT.Domain.Entities;
using GYMPT.Domain.Ports;
using Microsoft.Extensions.Logging; // Añadido para consistencia en el logging
using System; // Añadido para ArgumentNullException
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GYMPT.Application.Services
{
    public class DisciplineService : IDisciplineService
    {
        private readonly IDisciplineRepository _disciplineRepository;
        private readonly ILogger<DisciplineService> _logger;

        // Inyectamos el logger para mantener la consistencia con los otros servicios
        public DisciplineService(IDisciplineRepository disciplineRepository, ILogger<DisciplineService> logger)
        {
            _disciplineRepository = disciplineRepository ?? throw new ArgumentNullException(nameof(disciplineRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // --- LOS MÉTODOS AHORA IMPLEMENTAN EL CONTRATO ESTANDARIZADO ---

        public async Task<Discipline?> GetByIdAsync(int id)
        {
            _logger.LogInformation("Obteniendo disciplina con Id {DisciplineId}", id);
            return await _disciplineRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Discipline>> GetAllAsync()
        {
            _logger.LogInformation("Obteniendo todas las disciplinas");
            return await _disciplineRepository.GetAllAsync();
        }

        public async Task<Discipline> CreateAsync(Discipline discipline)
        {
            ArgumentNullException.ThrowIfNull(discipline, nameof(discipline));
            _logger.LogInformation("Creando nueva disciplina: {DisciplineName}", discipline.Name);
            return await _disciplineRepository.CreateAsync(discipline);
        }

        public async Task<Discipline?> UpdateAsync(Discipline discipline)
        {
            ArgumentNullException.ThrowIfNull(discipline, nameof(discipline));
            _logger.LogInformation("Actualizando disciplina con Id {DisciplineId}", discipline.Id);
            return await _disciplineRepository.UpdateAsync(discipline);
        }

        public async Task<bool> DeleteByIdAsync(int id)
        {
            _logger.LogInformation("Eliminando disciplina con Id {DisciplineId}", id);
            return await _disciplineRepository.DeleteByIdAsync(id);
        }
    }
}