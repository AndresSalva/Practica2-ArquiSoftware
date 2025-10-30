using Microsoft.AspNetCore.Hosting;
using ReportService.Application.Interfaces;

namespace GYMPT.Infrastructure.Providers;

public class LogoProvider : ILogoProvider
{
    private readonly IWebHostEnvironment _environment;

    public LogoProvider(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<byte[]> GetLogoAsync()
    {
        try
        {
            var logoPath = Path.Combine(_environment.ContentRootPath, "wwwroot", "images", "logo.jpeg");

            if (File.Exists(logoPath))
            {
                return await File.ReadAllBytesAsync(logoPath);
            }

            return CreateDefaultLogo();
        }
        catch
        {
            return CreateDefaultLogo();
        }
    }

    private byte[] CreateDefaultLogo()
    {
        string svgContent = @"
                <svg width='200' height='60' xmlns='http://www.w3.org/2000/svg'>
                    <rect width='200' height='60' fill='#1e40af' rx='5'/>
                    <text x='100' y='35' font-family='Arial' font-size='16' 
                          fill='white' text-anchor='middle' font-weight='bold'>GYMPT</text>
                </svg>";

        return System.Text.Encoding.UTF8.GetBytes(svgContent);
    }
}
