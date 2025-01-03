using System.Security.Cryptography;
using System.Text;
using Maestro.Data.Core;

namespace Maestro.Server.Services;

public class ApiKeyHasher : IApiKeyHasher
{
    public string Hash(string apiKey)
    {
        var apiKeyBytes = Encoding.UTF8.GetBytes(apiKey);
        var hashedApiKey = MD5.HashData(apiKeyBytes);
        var base64HashString = Convert.ToBase64String(hashedApiKey);
        return base64HashString.Length > DataConstraints.ApiKeyHashMaxLength ? base64HashString[..DataConstraints.ApiKeyHashMaxLength] : base64HashString;
    }
}