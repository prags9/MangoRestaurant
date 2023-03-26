﻿using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using System.Runtime.CompilerServices;

namespace Mango.Services.Identity
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

        public static IEnumerable<ApiScope> ApiScopes => new List<ApiScope>
        {
            new ApiScope("mango", "Mango Server"),
            new ApiScope("Read", "Read your data"),
            new ApiScope("Write", "Write your data"),
            new ApiScope("Delete", "Delete your data"),

        };
        public static IEnumerable<Client> Clients => new List<Client>
        {
            new Client
            {
                ClientId = "client",
                ClientSecrets = {new Secret("secret key Eg".Sha256())},
                AllowedGrantTypes =  GrantTypes.ClientCredentials,
                AllowedScopes = {"Read", "Write", "Delete", "profile"} //profile is built in
            },
             new Client
            {
                ClientId = "mango",
                ClientSecrets = {new Secret("secret key Eg".Sha256())},
                AllowedGrantTypes =  GrantTypes.Code,
                RedirectUris = { "https://localhost:7206/signin-oidc" },
                PostLogoutRedirectUris = { "https://localhost:7206/signout-callback-oidc" },
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "mango"
                }
            },
        };
    }
}
