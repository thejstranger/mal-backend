using System;
using System.Collections.Generic;
using System.Text;

namespace MAL.Common
{
    public interface IConfigProvider
    {
        string GetDbConnectionString();

        string GetJwtIssuer();
        string GetJwtKey();

        string GetActiveMqHost();
        string GetActiveMqPort();
        string GetActiveMqUser();
        string GetActiveMqPassword();
    }
}
