using Dapper;
using Npgsql;
using ServiceCommon.Domain.Ports;
using ReportService.Domain.Entities;
using ReportService.Application.Interfaces;

namespace ReportService.Application.Services;

public class ReportService : IReportService
{
    private readonly string _connectionString;
    private readonly IPdfReportBuilder _pdfBuilder;
    private readonly ILogoProvider _logoProvider; // Cambiar esta línea

    public ReportService(
        IConnectionStringProvider connectionStringProvider,
        IPdfReportBuilder pdfBuilder,
        ILogoProvider logoProvider) // Cambiar este parámetro
    {
        _connectionString = connectionStringProvider.GetPostgresConnection();
        _pdfBuilder = pdfBuilder;
        _logoProvider = logoProvider;
    }

    public async Task<PdfReport> GenerateInstructorPerformanceReportAsync()
    {
        const string sql = @"
            SELECT 
                u.specialization AS Especializacion,
                COUNT(d.id) AS DisciplinasImpartidas,
                COUNT(DISTINCT cm.id_client) AS ClientesAtendidos,
                COALESCE(SUM(m.price), 0) AS IngresosGenerados
            FROM public.user u
            LEFT JOIN discipline d ON u.id_person = d.id_user
            LEFT JOIN membership_disciplines md ON d.id = md.id_discipline
            LEFT JOIN membership m ON md.id_membership = m.id
            LEFT JOIN client_membership cm ON m.id = cm.id_membership
            WHERE u.role LIKE '%instructor%' OR u.specialization IS NOT NULL
            GROUP BY u.specialization
            ORDER BY IngresosGenerados DESC";
        
        using var connection = new NpgsqlConnection(_connectionString);
        var data = (await connection.QueryAsync<InstructorPerformanceReport>(sql)).ToList();

        // Cargar el logo usando el provider
        byte[] logoBytes = await _logoProvider.GetLogoAsync();

        var report = _pdfBuilder
            .SetTitle("Reporte de Rendimiento de Instructores")
            .SetData(data)
            .SetLogo(logoBytes)
            .AddHeader()
            .AddBody()
            .AddFooter()
            .Build();

        return report;
    }
}