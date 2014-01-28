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
    using System;
    using System.Collections.Generic;
    using Microsoft.Samples.SocialGames.Common.JobEngine;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Repositories;

    public class InviteCommand : ICommand
    {
        private INotificationRepository notificationRepository;
        private IUserRepository userRepository;

        public InviteCommand(INotificationRepository notificationRepository, IUserRepository userRepository)
        {
            this.notificationRepository = notificationRepository;
            this.userRepository = userRepository;
        }

        public void Do(IDictionary<string, object> context)
        {
            var userId = context["userId"].ToString();
            var gameQueueid = (Guid)context["gameQueueId"];
            var invitedUserId = context["invitedUserId"].ToString();
            var timestamp = (DateTime)context["timestamp"];
            var message = context["message"].ToString();
            var url = context.ContainsKey("url") ? context["url"].ToString() : null;

            var profile = this.userRepository.GetUser(userId);

            var notification = new Notification()
            {
                Id = Guid.NewGuid(),
                DateTime = timestamp,
                Message = message,
                Data = gameQueueid.ToString(),
                Url = url,
                Type = "Invite",                
                SenderId = userId,
                SenderName = profile != null && string.IsNullOrEmpty(profile.DisplayName) ? profile.DisplayName : userId
            };

            if (profile != null && profile.DisplayName != null)
            {
                notification.SenderName = profile.DisplayName;
            }
            else
            {
                notification.SenderName = userId;
            }

            this.notificationRepository.AddNotification(invitedUserId, notification);
        }
    }
}
