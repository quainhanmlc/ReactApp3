namespace ReactApp3.Server.Models
{
    namespace ReactLogin.Server.Models
    {
        public class JwtSettings
        {
            public string SecretKey { get; set; }
            public string Issuer { get; set; }
            public string Audience { get; set; }
        }
    }

}
