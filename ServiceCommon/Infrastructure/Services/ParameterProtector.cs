using Microsoft.AspNetCore.DataProtection;

namespace ServiceCommon.Infrastructure.Services
{
    public class ParameterProtector
    {
        private readonly IDataProtector _urlProtector;

        public ParameterProtector(IDataProtectionProvider provider)
        {
            _urlProtector = provider.CreateProtector("UrlTokenProtector");
        }

        public string Protect(string value)
            => _urlProtector.Protect(value);

        public string? Unprotect(string token)
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
