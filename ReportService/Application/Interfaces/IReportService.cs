using ReportService.Domain.Entities;

namespace ReportService.Application.Interfaces;

public interface IReportService
{
    Task<PdfReport> GenerateInstructorPerformanceReportAsync();
}