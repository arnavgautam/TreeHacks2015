// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

namespace Microsoft.Samples.SocialGames.Web.Services
{
    using System;
    using System.Json;
    using System.Net;
    using System.Net.Http;
    using System.Security.Permissions;
    using System.Text;
    using System.Web;
    using Microsoft.Samples.SocialGames.Repositories;

    public class AuthService : ServiceBase, IAuthService
    {
        private readonly IIdentityProviderRepository identityProviderRepository;
        private readonly HttpContextBase context;

        public AuthService(IIdentityProviderRepository identityProviderRepository)
            : this(new HttpContextWrapper(HttpContext.Current), identityProviderRepository)
        {
        }

        public AuthService(HttpContextBase context, IIdentityProviderRepository identityProviderRepository)
        {
            this.context = context;
            this.identityProviderRepository = identityProviderRepository;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public string LoginSelector(string returnUrl)
        {
            var realm = this.GetRealm();
            var identityProvidersInfo = this.identityProviderRepository.GetIdentityProvidersInfoEndpoint(realm, returnUrl ?? string.Empty);

            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var info = client.DownloadString(identityProvidersInfo);
                var json = JsonValue.Parse(info);

                return json.ToString();
            }
        }

        private string GetRealm()
        {
            var request = this.context.Request;
            var realm = new StringBuilder();

            realm.Append(request.Url.Scheme);
            realm.Append("://");
            realm.Append(request.Headers["Host"] ?? request.Url.Authority);
            realm.Append(request.ApplicationPath);

            if (!request.ApplicationPath.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                realm.Append("/");
            }

            return realm.ToString();
        }
    }
}