namespace TodoList.Config;
public class JwtConfigs
    {
        public string Secret { get; set; } = string.Empty;
        public int ExpirationInHour { get; set; }
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
    }