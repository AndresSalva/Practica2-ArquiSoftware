using Microsoft.AspNetCore.DataProtection;

namespace ServiceCommon.Infrastructure.Services
{
    /// <summary>
    /// Makes and Recovers tokens for URLs.
    /// Implemented as a singleton to ensure consistent protection across the application.
    /// The singleton instance should be configured in the DI container at application startup. AddSingleton()
    /// </summary>
    public class UrlTokenSingleton
    {
        private readonly IDataProtector _urlProtector;

        public UrlTokenSingleton(IDataProtectionProvider provider)
        {
            _urlProtector = provider.CreateProtector("UrlTokenProtector");
        }

        public string GenerateToken(string value)
            => _urlProtector.Protect(value);

        public string? GetTokenData(string token)
        {
            try
            {
                return _urlProtector.Unprotect(token);
            }
            catch
            {
                return null;
            }
        }
    }
}
