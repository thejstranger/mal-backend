using System;
using System.Collections.Generic;

namespace MAL.Common
{
    public class ConfigProvider : IConfigProvider
    {

        public string GetDbConnectionString()
        {
            var dbString = Environment.GetEnvironmentVariable("DB-CONNECTION-STRING") ?? "Host=localhost;Port=5432;Database=postgres;User Id=postgres;Password=password123;";

            return dbString;
        }

        public string GetJwtIssuer()
        {
            return Environment.GetEnvironmentVariable("JWT_ISSUER")
                   ?? "http://127.0.0.1:5321";
        }

        public string GetJwtAudience()
        {
            return Environment.GetEnvironmentVariable("JWT_AUDIENCE")
                   ?? "http://127.0.0.1:4321";
        }

        public string GetJwtKey()
        {
            return Environment.GetEnvironmentVariable("JWT_KEY")
                   ?? "dev_JWT_key12345";
        }

        public string GetActiveMqHost()
        {
            return Environment.GetEnvironmentVariable("AMQ_HOST")
                   ?? "localhost";
        }

        public string GetActiveMqPort()
        {
            return Environment.GetEnvironmentVariable("AMQ_PORT")
                   ?? "61616";
        }

        public string GetActiveMqUser()
        {
            return Environment.GetEnvironmentVariable("AMQ_USER")
                   ?? "admin";
        }

        public string GetActiveMqPassword()
        {
            return Environment.GetEnvironmentVariable("AMQ_PASSWORD")
                   ?? "admin";
        }
    }
}
