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

namespace Microsoft.Samples.SocialGames.Worker.Commands
{
    using System.Collections.Generic;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Repositories;

    public class GameActionStatisticsCommand : GameActionCommand
    {
        private readonly IStatisticsRepository statisticsRepository;

        public GameActionStatisticsCommand(IStatisticsRepository statisticsRepository)
        {
            this.statisticsRepository = statisticsRepository;
        }

        public override void Do(GameAction gameAction)
        {
            if (string.IsNullOrWhiteSpace(gameAction.UserId))
            {
                return;
            }

            // Retrieve existent statistics and update them with the new results
            UserStats currentStatistics = this.statisticsRepository.Retrieve(gameAction.UserId);
            currentStatistics = UpdateCurrentStats(gameAction, currentStatistics);
            
            // Generate a new UserStats with the updated RowKey to add in the TableStorage
            UserStats statistics = null;
            statistics = CreateUpdatedStats(currentStatistics);
            statistics.PartitionKey = (int.MaxValue - statistics.Victories).ToString().PadLeft(int.MaxValue.ToString().Length);
            statistics.RowKey = statistics.UserId;
            this.statisticsRepository.Save(statistics);
        }

        private static UserStats UpdateCurrentStats(GameAction gameAction, UserStats originalStatistics)
        {
            if (originalStatistics == null)
            {
                originalStatistics = new UserStats();
                originalStatistics.UserId = gameAction.UserId;
            }

            originalStatistics.Victories += GetValue(gameAction.CommandData, "Victories");
            originalStatistics.Defeats += GetValue(gameAction.CommandData, "Defeats");
            originalStatistics.GameCount += GetValue(gameAction.CommandData, "GameCount");
            return originalStatistics;
        }

        private static UserStats CreateUpdatedStats(UserStats originalStatistics)
        {
            var statistics = new UserStats
            {
                UserId = originalStatistics.UserId,
                Victories = originalStatistics.Victories,
                Defeats = originalStatistics.Defeats,
                GameCount = originalStatistics.GameCount
            };
            return statistics;
        }

        private static int GetValue(IDictionary<string, object> commandData, string key)
        {
            var value = 0;

            if (commandData.ContainsKey(key))
            {
                int.TryParse(commandData[key].ToString(), out value);
            }

            return value;
        }
   }
}