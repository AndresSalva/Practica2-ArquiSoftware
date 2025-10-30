namespace ReportService.Domain.Entities;

public class InstructorPerformanceReport
{
    public string Especializacion { get; set; }
    public int DisciplinasImpartidas { get; set; }
    public int ClientesAtendidos { get; set; }
    public decimal IngresosGenerados { get; set; }
}