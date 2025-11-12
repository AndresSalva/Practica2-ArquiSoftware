using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReportService.Application.Interfaces;

namespace GYMPT.Pages.Reports
{
    public class IndexModel : PageModel
    {
        private readonly IReportService _reportService;
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _env;

        public IndexModel(IReportService reportService, ILogger<IndexModel> logger, IWebHostEnvironment env)
        {
            _reportService = reportService;
            _logger = logger;
            _env = env;
        }

        [BindProperty]
        public string ReportType { get; set; }

        [BindProperty]
        public string Format { get; set; }

        [TempData]
        public string SuccessMessage { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string FileName { get; set; }

        public bool ShowPreview => !string.IsNullOrEmpty(FileName);

        public void OnGet()
        {
            // Si hay un archivo temporal existente, simplemente mostrar el preview
        }

        public async Task<IActionResult> OnPostGenerateReportAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(ReportType) || string.IsNullOrEmpty(Format))
                {
                    ErrorMessage = "Por favor, seleccione el tipo de reporte y formato.";
                    return RedirectToPage();
                }

                string reportsDir = Path.Combine(_env.WebRootPath, "temp", "reports");
                if (!Directory.Exists(reportsDir))
                    Directory.CreateDirectory(reportsDir);

                byte[] reportBytes;
                string contentType;
                string extension;

                switch (ReportType.ToLower())
                {
                    case "instructorperformance":
                        var pdfReport = await _reportService.GenerateInstructorPerformanceReportAsync();
                        reportBytes = pdfReport.Content;
                        contentType = "application/pdf";
                        extension = "pdf";
                        break;

                    default:
                        ErrorMessage = "Tipo de reporte no implementado.";
                        return RedirectToPage();
                }

                string fileName = $"Reporte_{ReportType}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.{extension}";
                string filePath = Path.Combine(reportsDir, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, reportBytes);

                SuccessMessage = "Reporte generado exitosamente.";
                return RedirectToPage(new { fileName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generando reporte: {ReportType}, {Format}", ReportType, Format);
                ErrorMessage = $"Error al generar el reporte: {ex.Message}";
                return RedirectToPage();
            }
        }

        public IActionResult OnGetDownloadReport(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    ErrorMessage = "No se especificó el archivo para descargar.";
                    return RedirectToPage();
                }

                string filePath = Path.Combine(_env.WebRootPath, "temp", "reports", fileName);
                if (!System.IO.File.Exists(filePath))
                {
                    ErrorMessage = "El archivo solicitado no existe o ha expirado.";
                    return RedirectToPage();
                }

                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error descargando reporte {FileName}", fileName);
                ErrorMessage = $"Error al descargar el reporte: {ex.Message}";
                return RedirectToPage();
            }
        }

        public string GetReportBase64()
        {
            try
            {
                if (string.IsNullOrEmpty(FileName))
                    return string.Empty;

                string fullPath = Path.Combine(_env.WebRootPath, "temp", "reports", FileName);
                if (!System.IO.File.Exists(fullPath))
                    return string.Empty;

                var bytes = System.IO.File.ReadAllBytes(fullPath);
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
