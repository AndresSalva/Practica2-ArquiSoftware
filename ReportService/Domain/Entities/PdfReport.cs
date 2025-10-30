namespace ReportService.Domain.Entities;

public class PdfReport
{
    public string Title { get; set; }
    public DateTime GeneratedAt { get; set; }
    public List<InstructorPerformanceReport> Data { get; set; }
    public byte[] Content { get; set; }
}
