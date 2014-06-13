namespace MobileService.Common
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using System.ServiceModel.Security.Tokens;
    using System.Text;
    using Microsoft.WindowsAzure.Mobile.Service.Security;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    /// <summary>
    /// Contains helper functions for generating and operating on authentication tokens
    /// </summary>
    internal class TokenUtility
    {
        private const string ZumoIssuerValue = "urn:microsoft:windows-azure:zumo";
        private const string ProviderCredentialsClaimName = "urn:microsoft:credentials";
        private const string ZumoAudienceValue = ZumoIssuerValue; // TODO: what should this be?

        private static readonly TokenUtility TokenUtilityInstance = new TokenUtility();
        private readonly JsonSerializerSettings tokenSerializerSettings;

        public TokenUtility()
        {
            this.tokenSerializerSettings = GetTokenSerializerSettings();
        }

        public static TokenUtility Instance
        {
            get
            {
                return TokenUtilityInstance;
            }
        }

        public virtual JwtSecurityToken CreateLoginToken(string secretKey, ClaimsIdentity claimsIdentity, ProviderCredentials providerCredentials)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("secretKey");
            }

            if (claimsIdentity == null)
            {
                throw new ArgumentNullException("claimsIdentity");
            }

            if (providerCredentials == null)
            {
                throw new ArgumentNullException("providerCredentials");
            }

            var providerKeyClaim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (providerKeyClaim == null)
            {
                throw new ArgumentException("RResources.Token_Invalid.FormatForUser(claimsIdentity.Name, ClaimTypes.NameIdentifier)");
            }

            var uid = providerKeyClaim.Value;
            var credentialsClaimJson = JsonConvert.SerializeObject(providerCredentials, Formatting.None, this.tokenSerializerSettings);

            var claims = new List<Claim>();
            claims.Add(new Claim(ProviderCredentialsClaimName, credentialsClaimJson));
            claims.Add(new Claim("uid", uid));
            claims.Add(new Claim("ver", "1"));

            return this.CreateTokenFromClaims(claims, secretKey, ZumoAudienceValue, ZumoIssuerValue);
        }

        protected virtual JwtSecurityToken CreateTokenFromClaims(IEnumerable<Claim> claims, string secretKey, string audience, string issuer)
        {
            var signingKey = this.GetSigningKey(secretKey);
            var signingToken = new BinarySecretSecurityToken(signingKey);
            var signingCredentials = new SigningCredentials(new InMemorySymmetricSecurityKey(signingToken.GetKeyBytes()), "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256", "http://www.w3.org/2001/04/xmlenc#sha256");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                AppliesToAddress = audience,
                TokenIssuerName = issuer,
                SigningCredentials = signingCredentials,
                Lifetime = new Lifetime(DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(1)),
                Subject = new ClaimsIdentity(claims),
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor) as JwtSecurityToken;

            return token;
        }

        protected virtual byte[] GetSigningKey(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentNullException("secretKey");
            }

            var encoder = new UTF8Encoding(true, true);
            var computeHashInput = encoder.GetBytes(secretKey + "JWTSig");
            byte[] signingKey;

            using (var sha256Provider = new SHA256Managed())
            {
                signingKey = sha256Provider.ComputeHash(computeHashInput);
            }

            return signingKey;
        }
        
        private static JsonSerializerSettings GetTokenSerializerSettings()
        {
            return new JsonSerializerSettings()
            {
                DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),

                // Setting this to None prevents Json.NET from loading malicious, unsafe, or security-sensitive types
                TypeNameHandling = TypeNameHandling.None
            };
        }
    }
}
