using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReportService.Application.Interfaces;

namespace GYMPT.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IReportService reportService, ILogger<IndexModel> logger)
        {
            _reportService = reportService;
            _logger = logger;
        }

        [BindProperty]
        public string ReportType { get; set; }

        [BindProperty]
        public string Format { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        // Nueva propiedad para almacenar el reporte generado
        public byte[] GeneratedReport { get; set; }
        public string ContentType { get; set; }
        public string FileName { get; set; }
        public bool ShowPreview { get; set; }

        public void OnGet()
        {
            // Inicializar valores por defecto si es necesario
        }

        public async Task<IActionResult> OnPostGenerateReportAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(ReportType) || string.IsNullOrEmpty(Format))
                {
                    ErrorMessage = "Por favor, seleccione el tipo de reporte y formato.";
                    return Page();
                }

                switch (ReportType.ToLower())
                {
                    case "instructorperformance":
                        var pdfReport = await _reportService.GenerateInstructorPerformanceReportAsync();
                        GeneratedReport = pdfReport.Content;
                        ContentType = "application/pdf";
                        FileName = $"Reporte_Instructores_{DateTime.UtcNow:yyyyMMdd_HHmmss}.pdf";
                        SuccessMessage = "Reporte de rendimiento de instructores generado exitosamente.";
                        break;

                    default:
                        ErrorMessage = "Tipo de reporte no implementado.";
                        return Page();
                }

                // En lugar de descargar, mostrar preview
                ShowPreview = true;
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte: {ReportType}, Formato: {Format}", 
                    ReportType, Format);
                
                ErrorMessage = $"Error al generar el reporte: {ex.Message}";
                return Page();
            }
        }

        // Nuevo método para descargar el reporte
        public IActionResult OnGetDownloadReport()
        {
            try
            {
                if (GeneratedReport == null || GeneratedReport.Length == 0)
                {
                    ErrorMessage = "No hay reporte generado para descargar.";
                    return RedirectToPage();
                }

                return File(GeneratedReport, ContentType, FileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando reporte");
                ErrorMessage = $"Error al descargar el reporte: {ex.Message}";
                return RedirectToPage();
            }
        }

        // Método para obtener el PDF como base64 para el preview
        public string GetReportBase64()
        {
            if (GeneratedReport == null || GeneratedReport.Length == 0)
                return string.Empty;

            return Convert.ToBase64String(GeneratedReport);
        }
    }
}