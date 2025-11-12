using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ReportService.Domain.Entities;
using ReportService.Application.Interfaces;

namespace ReportService.Infrastructure;

public class InstructorPerformancePdfBuilder : IPdfReportBuilder
{
    private PdfReport _report;
    private List<InstructorPerformanceReport> _data;
    private byte[] _logoBytes;

    public InstructorPerformancePdfBuilder()
    {
        _report = new PdfReport
        {
            GeneratedAt = DateTime.UtcNow
        };
    }

    public IPdfReportBuilder SetTitle(string title)
    {
        _report.Title = title;
        return this;
    }

    public IPdfReportBuilder SetData<T>(IEnumerable<T> data)
    {
        _data = data as List<InstructorPerformanceReport>;
        return this;
    }

    public IPdfReportBuilder SetLogo(byte[] logoBytes)
    {
        _logoBytes = logoBytes;
        return this;
    }

    public IPdfReportBuilder AddHeader()
    {
        return this;
    }

    public IPdfReportBuilder AddBody()
    {
        return this;
    }

    public IPdfReportBuilder AddFooter()
    {
        return this;
    }

    [Obsolete]
    public PdfReport Build()
    {
        var document = Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(2, Unit.Centimetre);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(11));

                // HEADER MEJORADO CON LOGO
                page.Header().Element(ComposeHeader);

                // CONTENIDO PRINCIPAL
                page.Content()
                    .PaddingVertical(1, Unit.Centimetre)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        // Tabla de datos
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(2); // Especialización
                                columns.RelativeColumn(1.5f); // Disciplinas
                                columns.RelativeColumn(1.5f); // Clientes
                                columns.RelativeColumn(2); // Ingresos
                            });

                            // Encabezado de la tabla
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Blue.Darken3).Padding(8)
                                    .Text("Especialización").FontColor(Colors.White).Bold().FontSize(10);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(8)
                                    .Text("Disciplinas").FontColor(Colors.White).Bold().FontSize(10);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(8)
                                    .Text("Clientes").FontColor(Colors.White).Bold().FontSize(10);
                                header.Cell().Background(Colors.Blue.Darken3).Padding(8)
                                    .Text("Ingresos Generados").FontColor(Colors.White).Bold().FontSize(10);
                            });

                            // Datos
                            foreach (var item in _data)
                            {
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text(item.Especializacion ?? "Sin especialización").FontSize(9);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text(item.DisciplinasImpartidas.ToString()).AlignRight().FontSize(9);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text(item.ClientesAtendidos.ToString()).AlignRight().FontSize(9);
                                table.Cell().BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(6)
                                    .Text($"${item.IngresosGenerados:N2}").AlignRight().FontSize(9);
                            }

                            // Totales
                            if (_data.Any())
                            {
                                table.Cell().ColumnSpan(3).Background(Colors.Grey.Lighten3).Padding(6)
                                    .Text("TOTAL").SemiBold().AlignRight().FontSize(10);
                                table.Cell().Background(Colors.Grey.Lighten3).Padding(6)
                                    .Text($"${_data.Sum(x => x.IngresosGenerados):N2}").SemiBold().AlignRight().FontSize(10);
                            }
                        });
                    });

                // FOOTER MEJORADO CON FECHA
                page.Footer().Element(ComposeFooter);
            });
        });

        _report.Content = document.GeneratePdf();
        return _report;
    }

    [Obsolete]
    private void ComposeHeader(IContainer container)
    {
        container.Row(row =>
        {
            // Logo (si está disponible)
            if (_logoBytes != null && _logoBytes.Length > 0)
            {
                row.RelativeItem(2).Height(60).Image(_logoBytes, ImageScaling.FitArea);
            }
            else
            {
                // Placeholder si no hay logo
                row.RelativeItem(2).Height(60).Background(Colors.Grey.Lighten3)
                   .AlignCenter().AlignMiddle().Text("LOGO").Bold().FontColor(Colors.Grey.Medium);
            }

            // Título del reporte
            row.RelativeItem(5).Column(column =>
            {
                column.Item().AlignCenter().Text(_report.Title ?? "Reporte de Rendimiento de Instructores")
                    .SemiBold().FontSize(16).FontColor(Colors.Blue.Darken3);

                column.Item().AlignCenter().Text("GYMPT - Sistema de Gestión Deportiva")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            });

            // Espacio para balancear el layout
            row.RelativeItem(2);
        });
    }

    private void ComposeFooter(IContainer container)
    {
        container.Row(row =>
        {
            // Información de la empresa
            row.RelativeItem(3).Column(column =>
            {
                column.Item().Text("GYMPT Sports Center")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                column.Item().Text("Tel: +1 234-567-8900")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
            });

            // Fecha y información de generación
            row.RelativeItem(4).AlignCenter().Column(column =>
            {
                column.Item().Text(text =>
                {
                    text.Span("Generado el: ").SemiBold().FontSize(8);
                    text.Span(_report.GeneratedAt.ToString("dd/MM/yyyy 'a las' HH:mm")).FontSize(8);
                });
                
                column.Item().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber().SemiBold();
                    text.Span(" de ");
                    text.TotalPages().SemiBold();
                });
            });

            // Información confidencial
            row.RelativeItem(3).AlignRight().Column(column =>
            {
                column.Item().Text("Uso Interno")
                    .FontSize(8).FontColor(Colors.Grey.Darken1);
                column.Item().Text("Confidencial")
                    .FontSize(8).FontColor(Colors.Red.Medium);
            });
        });
    }
}