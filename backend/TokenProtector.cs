using Microsoft.AspNetCore.DataProtection;

namespace backend;

public class TokenProtector
{
    private readonly IDataProtector _protector;

    public TokenProtector(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("HomeAssistantTokenProtector");
    }

    public string Protect(string token)
    {
        return _protector.Protect(token);
    }

    public string Unprotect(string encryptedToken)
    {
        return _protector.Unprotect(encryptedToken);
    }
}
