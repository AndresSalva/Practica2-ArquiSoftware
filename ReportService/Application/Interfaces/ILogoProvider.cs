namespace ReportService.Application.Interfaces;

public interface ILogoProvider
{
    Task<byte[]> GetLogoAsync();
}
