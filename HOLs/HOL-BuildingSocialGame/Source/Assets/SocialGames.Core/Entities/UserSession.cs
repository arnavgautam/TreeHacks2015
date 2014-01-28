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

namespace Microsoft.Samples.SocialGames.Entities
{
    using System;

    public class UserSession
    {
        public string UserId { get; set; }

        public Guid ActiveGameQueueId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.Equals((UserSession)obj);
        }

        public bool Equals(UserSession userSession)
        {
            return this.UserId == userSession.UserId && this.ActiveGameQueueId == userSession.ActiveGameQueueId;
        }

        public override int GetHashCode()
        {
            return this.UserId.GetHashCode() ^ this.ActiveGameQueueId.GetHashCode();
        }
    }
}