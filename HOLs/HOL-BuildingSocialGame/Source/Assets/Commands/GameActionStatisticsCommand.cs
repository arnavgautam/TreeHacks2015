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
            // TODO: Add save statistics functionality
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