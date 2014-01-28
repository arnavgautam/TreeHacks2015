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

namespace Microsoft.Samples.SocialGames.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Security.Permissions;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using Microsoft.Samples.SocialGames.Common.Storage;

    public class StatisticsRepository : IStatisticsRepository, IStorageInitializer
    {
        public StatisticsRepository(IAzureTable<UserStats> statsTable)
        {
            // TODO: Add Constructor logic
        }

        public static string EncodeKey(string key)
        {
            if (key == null)
            {
                return null;
            }

            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(key)).Replace("/", "_");
        }

        public static string DecodeKey(string encodedKey)
        {
            if (encodedKey == null)
            {
                return null;
            }

            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedKey.Replace("_", "/")));
        }

        public void Save(UserStats stats)
        {
            throw new NotImplementedException();
        }

        public UserStats Retrieve(string userId)
        {
            throw new NotImplementedException();
        }

        public Board GenerateLeaderboard(int focusCount)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            // TODO: Add code
        }
    }
}
