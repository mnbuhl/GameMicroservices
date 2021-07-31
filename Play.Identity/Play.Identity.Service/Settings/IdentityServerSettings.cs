using System;
using System.Collections.Generic;
using IdentityServer4.Models;

namespace Play.Identity.Service.Settings
{
    public class IdentityServerSettings
    {
        public IReadOnlyCollection<ApiScope> ApiScopes { get; set; } = Array.Empty<ApiScope>();
        public IReadOnlyCollection<Client> Clients { get; set; } = Array.Empty<Client>();
    }
}