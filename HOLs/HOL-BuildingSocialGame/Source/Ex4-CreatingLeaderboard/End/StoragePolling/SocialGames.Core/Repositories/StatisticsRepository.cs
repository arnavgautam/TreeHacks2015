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
        private readonly IAzureTable<UserStats> statsTable;

        public StatisticsRepository(IAzureTable<UserStats> statsTable)
        {
            if (statsTable == null)
            {
                throw new ArgumentNullException("statsTable");
            }

            this.statsTable = statsTable;
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
            stats.RowKey = EncodeKey(stats.UserId);
            UserStats currentStat = this.statsTable.Query.Where(item => item.RowKey == stats.RowKey).FirstOrDefault();
            if (currentStat != null)
            {
                this.statsTable.DeleteEntity(currentStat);
            }

            this.statsTable.AddEntity(stats);
        }

        public UserStats Retrieve(string userId)
        {
            return this.statsTable.Query.Where(item => item.RowKey.Equals(EncodeKey(userId))).FirstOrDefault();
        }

        public Board GenerateLeaderboard(int focusCount)
        {
            int id = 0;
            var board = new Board()
            {
                Id = ++id,
                Name = "Victories",
                Scores = null
            };
            UserStats[] data = this.statsTable.Query.Take(focusCount).ToArray();
            board.Scores = new Score[data.Count()];
            int a = 0;
            foreach (UserStats stats in data)
            {
                board.Scores[a] = new Score()
                {
                    Id = ++a,
                    UserId = stats.UserId,
                    Victories = stats.Victories,
                    Defeats = stats.Defeats,
                    GameCount = stats.GameCount
                };
            }

            return board;
        }

        public void Initialize()
        {
            this.statsTable.CreateIfNotExist();
        }
    }
}
