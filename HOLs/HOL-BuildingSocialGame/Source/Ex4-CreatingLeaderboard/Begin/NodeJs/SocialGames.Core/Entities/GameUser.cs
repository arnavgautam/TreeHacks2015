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
    using System.Collections.Generic;
    using Microsoft.Samples.SocialGames.Helpers;

    public class GameUser
    {
        public GameUser()
        {
            this.Weapons = new List<Guid>();
        }
        
        public string UserId { get; set; }

        public string UserName { get; set; }
        
        public List<Guid> Weapons { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is GameUser) && this.Equals((GameUser)obj);
        }

        public bool Equals(GameUser gameUser)
        {
            bool result = this.UserId == gameUser.UserId &&
                   this.UserName == gameUser.UserName &&
                   new CollectionHaveSameElementsComparison<Guid>(this.Weapons, gameUser.Weapons).DoIt();

            return result;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}