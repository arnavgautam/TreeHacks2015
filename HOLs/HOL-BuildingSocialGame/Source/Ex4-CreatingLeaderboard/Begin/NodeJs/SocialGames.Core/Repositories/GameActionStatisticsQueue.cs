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
    using Microsoft.Samples.SocialGames.Common.Storage;
    using Microsoft.Samples.SocialGames.Entities;

    public class GameActionStatisticsQueue : IGameActionStatisticsQueue, IStorageInitializer
    {
        private readonly IAzureQueue<GameActionStatisticsMessage> gameActionStatisticGameQueue;

        public GameActionStatisticsQueue(IAzureQueue<GameActionStatisticsMessage> gameActionStatisticsQueue)
        {
            if (gameActionStatisticsQueue == null)
            {
                throw new ArgumentNullException("gameActionStatisticsQueue");
            }

            this.gameActionStatisticGameQueue = gameActionStatisticsQueue;
        }

        public void Add(GameAction gameAction)
        {
            if (gameAction == null)
            {
                throw new ArgumentException("gameAction");
            }

            GameActionStatisticsMessage message = new GameActionStatisticsMessage() { GameAction = gameAction };

            this.gameActionStatisticGameQueue.AddMessage(message);
        }

        public void Initialize()
        {
            this.gameActionStatisticGameQueue.EnsureExist();
        }
    }
}