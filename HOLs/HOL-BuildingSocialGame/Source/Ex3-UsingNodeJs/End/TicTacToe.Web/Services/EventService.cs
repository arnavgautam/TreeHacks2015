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
    using System.Collections.Generic;
    using System.Json;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Repositories;
    using Microsoft.Samples.SocialGames.Web.Extensions;

    public class EventService : ServiceBase, IEventService
    {
        private IGameActionNotificationQueue notificationQueue;
        private IGameActionStatisticsQueue statisticsQueue;

        public EventService(IGameActionNotificationQueue notificationQueue, IGameActionStatisticsQueue statisticsQueue, IUserProvider userProvider)
            : base(userProvider)
        {
            this.notificationQueue = notificationQueue;
            this.statisticsQueue = statisticsQueue;
        }

        public void PostEvent(string topic, dynamic formContent)
        {
            if (string.IsNullOrWhiteSpace(this.CurrentUserId))
            {
                throw new ServiceException("The user is not authenticated");
            }

            // Command Type
            int commandType;

            try
            {
                commandType = Convert.ToInt32(formContent.type.Value);
            }
            catch
            {
                throw new ServiceException("Invalid type parameter");
            }

            if (topic != "notifications" && topic != "statistics")
            {
                throw new ServiceException("Invalid topic parameter");
            }

            // Command Data
            var jsonCommandData = (JsonObject)(formContent.commandData ?? null);
            IDictionary<string, object> commandData = null;

            if (jsonCommandData != null)
            {
                commandData = jsonCommandData.ToDictionary();
            }

            // Add gameAction
            var gameAction = new GameAction
            {
                Id = Guid.NewGuid(),
                Type = commandType,
                CommandData = commandData,
                UserId = this.CurrentUserId,
                Timestamp = DateTime.UtcNow
            };

            if (topic == "notifications")
            {
                this.notificationQueue.Add(gameAction);
            }
            else if (topic == "statistics")
            {
                this.statisticsQueue.Add(gameAction);
            }
        }
    }
}
