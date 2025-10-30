using ReportService.Domain.Entities;

namespace ReportService.Application.Interfaces;

public interface IPdfReportBuilder
{
    IPdfReportBuilder SetTitle(string title);
    IPdfReportBuilder SetData<T>(IEnumerable<T> data);
    IPdfReportBuilder SetLogo(byte[] logoBytes);
    IPdfReportBuilder AddHeader();
    IPdfReportBuilder AddBody();
    IPdfReportBuilder AddFooter();
    PdfReport Build();
}
