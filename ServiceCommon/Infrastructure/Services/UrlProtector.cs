using Microsoft.AspNetCore.DataProtection;

namespace ServiceCommon.Infrastructure.Services
{
    public class UrlProtector
    {
        private readonly IDataProtector _urlProtector;
        
        public UrlProtector(IDataProtectionProvider provider)
        {
            _urlProtector = provider.CreateProtector("UrlTokenProtector");
        }

        public string ProtectUrl(string value)
            => _urlProtector.Protect(value);

        public string? UnprotectUrl(string token)
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
