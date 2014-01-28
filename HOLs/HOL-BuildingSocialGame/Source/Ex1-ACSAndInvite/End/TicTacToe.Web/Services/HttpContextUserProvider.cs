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
    using System.Linq;
    using System.Web;
    using Microsoft.IdentityModel.Claims;

    public class HttpContextUserProvider : IUserProvider
    {
        public string UserId
        {
            get
            {
                var wrapper = new HttpContextWrapper(HttpContext.Current);
                return
                    wrapper.User.Identity.IsAuthenticated ?
                    ((IClaimsIdentity)wrapper.User.Identity).Claims.Single(c => c.ClaimType == ClaimTypes.NameIdentifier).Value :
                    string.Empty;
            }
        }
    }
}