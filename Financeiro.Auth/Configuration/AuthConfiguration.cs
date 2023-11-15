namespace Financeiro.Auth.Configuration
{
    public class AuthConfiguration
    {
        public string KeySecret { get; set; } = string.Empty;
        public TimeSpan Expires { get; set; }
    }
}