namespace Nqey.Domain.Common
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = "Nqey";
        public List<string> Audiences { get; set; } = new() { "Swagger-Client" };
        public string SigningKey { get; set; } = "f5422e6cdfde4af3bf631c7ddf180b97!";
        public int ExpiryMinutes { get; set; } = 60;

    }
}
