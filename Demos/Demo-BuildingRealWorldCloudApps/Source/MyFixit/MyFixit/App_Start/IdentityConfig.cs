namespace MyFixIt
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel.Services;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Helpers;

    using MyFixIt.Utils;

    // For more information on ASP.NET Identity, please visit http://go.microsoft.com/fwlink/?LinkId=301863
    public static class IdentityConfig
    {
        public static string AudienceUri { get; private set; }

        public static string Realm { get; private set; }

        public static string ClaimsIssuer { get; set; }

        public static string UserNameClaimType { get; set; }

        public static string UserIdClaimType { get; set; }

        public static string GiveNameClaimType { get; set; }

        public static void ConfigureIdentity()
        {
            RefreshValidationSettings();

            // Set the realm for the application
            Realm = ConfigurationManager.AppSettings["ida:realm"];

            // Set the audienceUri for the application
            AudienceUri = ConfigurationManager.AppSettings["ida:AudienceUri"];
            if (!string.IsNullOrEmpty(AudienceUri))
            {
                UpdateAudienceUri();
            }

            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            ClaimsIssuer = System.Security.Claims.ClaimsIdentity.DefaultIssuer;
            UserIdClaimType = "http://schemas.microsoft.com/aspnet/userid";
            UserNameClaimType = "http://schemas.microsoft.com/aspnet/username";
            GiveNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname";
        }

        public static void RefreshValidationSettings()
        {
            string metadataLocation = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            DatabaseIssuerNameRegistry.RefreshKeys(metadataLocation);
        }

        public static void UpdateAudienceUri()
        {
            int count = FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                .AudienceRestriction.AllowedAudienceUris.Count(
                    uri => string.Equals(uri.OriginalString, AudienceUri, StringComparison.OrdinalIgnoreCase));
            if (count == 0)
            {
                FederatedAuthentication.FederationConfiguration.IdentityConfiguration
                    .AudienceRestriction.AllowedAudienceUris.Add(new Uri(IdentityConfig.AudienceUri));
            }
        }

        public static IList<Claim> RemoveUserIdentityClaims(IEnumerable<Claim> claims)
        {
            List<Claim> filteredClaims = new List<Claim>();
            foreach (var c in claims)
            {
                // Strip out any existing name/nameid claims
                if (c.Type != ClaimTypes.Name)
                {
                    filteredClaims.Add(c);
                }
            }

            return filteredClaims;
        }

        public static void AddUserIdentityClaims(string displayName, IList<Claim> claims)
        {
            claims.Add(new Claim(ClaimTypes.Name, displayName, ClaimsIssuer));
            claims.Add(new Claim(UserNameClaimType, displayName, ClaimsIssuer));
        }
    }
}
