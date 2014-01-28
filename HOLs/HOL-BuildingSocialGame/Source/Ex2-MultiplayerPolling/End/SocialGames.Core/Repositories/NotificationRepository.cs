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
    using System.Linq;
    using Microsoft.Samples.SocialGames.Common.Storage;
    using Microsoft.Samples.SocialGames.Entities;

    public class NotificationRepository : INotificationRepository, IStorageInitializer
    {
        private readonly IAzureBlobContainer<NotificationStatus> notificationsContainer;

        public NotificationRepository(IAzureBlobContainer<NotificationStatus> notificationsContainer)
        {
            if (notificationsContainer == null)
            {
                throw new ArgumentNullException("notificationsContainer");
            }

            this.notificationsContainer = notificationsContainer;          
        }

        public void Initialize()
        {
            this.notificationsContainer.EnsureExist(true);
        }

        public void AddNotification(string userId, Notification notification)
        {
            var status = this.notificationsContainer.Get(userId);

            if (status == null)
            {
                status = new NotificationStatus();
                status.Notifications = new List<Notification>();
            }

            var toremove = status.Notifications.Where(n => notification.DateTime.Subtract(n.DateTime).TotalSeconds >= ConfigurationConstants.NotificationTimeInterval).ToList();

            foreach (var item in toremove)
            {
                status.Notifications.Remove(item);
            }

            if (!status.Notifications.Any(n => n.SenderId == notification.SenderId))
            {
                status.Notifications.Add(notification);
            }

            this.notificationsContainer.Save(userId, status);
        }

        public void AddGlobalNotification(Notification notification)
        {
            throw new NotImplementedException();
        }

        public NotificationStatus GetNotificationStatus(string userId)
        {
            return this.notificationsContainer.Get(userId);
        }

        public NotificationStatus GetGlobalNotificationStatus()
        {
            throw new NotImplementedException();
        }
    }
}