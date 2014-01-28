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
    using Microsoft.Samples.SocialGames.Entities;

    public interface IGameRepository
    {
        void AddUserToGameQueue(string userId, Guid gameId);

        void AddOrUpdateGame(Game game);

        void AddOrUpdateGameQueue(GameQueue gameQueue);

        Game GetGame(Guid gameId);

        GameQueue GetGameQueue(Guid gameQueueId);

        Game StartGame(Guid gameQueueId);

        string GetGameReference(Guid gameId, TimeSpan expiryTime);

        void Invite(string userId, Guid gameQueueId, string message, string url, List<string> users);
    }
}