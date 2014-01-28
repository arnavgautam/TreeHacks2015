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
    using System.Linq;
    using System.Net.Http;
    using System.Web.Mvc;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Repositories;
    using Microsoft.Samples.SocialGames.Web.Extensions;

    public class GameService : ServiceBase, IGameService
    {
        private readonly IGameRepository gameRepository;
        private readonly IUserRepository userRepository;

        public GameService(IGameRepository gameRepository, IUserRepository userRepository, IUserProvider userProvider)
            : base(userProvider)
        {            
            this.gameRepository = gameRepository;
            this.userRepository = userRepository;
        }

        public string Create()
        {
            Guid gameQueueId = Guid.NewGuid();
            string userId = this.CurrentUserId;

            if (string.IsNullOrEmpty(userId))
            {
                throw new ServiceException("User does not exist. User Id: " + userId);
            }

            UserProfile profile = this.userRepository.GetUser(CurrentUserId);

            if (profile == null)
            {
                throw new ServiceException("User does not exist. User Id: " + userId);
            }

            GameUser user = new GameUser()
            {
                UserId = profile.Id,
                UserName = profile.DisplayName,
                Weapons = new List<Guid>()
            };

            GameQueue gameQueue = new GameQueue()
            {
                Id = gameQueueId,
                CreationTime = DateTime.UtcNow,
                Status = QueueStatus.Waiting,
                Users = new List<GameUser>() { user }
            };

            this.gameRepository.AddOrUpdateGameQueue(gameQueue);

            return gameQueueId.ToString();
        }

        public void Join(Guid gameQueueId)
        {
            this.gameRepository.AddUserToGameQueue(CurrentUserId, gameQueueId);

            var queue = this.gameRepository.GetGameQueue(gameQueueId);
            var currentUserId = this.CurrentUserId;

            foreach (var user in queue.Users)
            {
                this.userRepository.AddFriend(currentUserId, user.UserId);
                this.userRepository.AddFriend(user.UserId, currentUserId);
            }
        }

        public void Start(Guid gameQueueId)
        {
            this.gameRepository.StartGame(gameQueueId);
        }

        public void Command(Guid gameId, dynamic formContent)
        {
            var game = this.gameRepository.GetGame(gameId);
            
            if (game == null)
            {
                throw new ServiceException("Game does not exist. Game Id: " + gameId);
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

            game.GameActions.Add(gameAction);
            this.gameRepository.AddOrUpdateGame(game);
        }

        public void Invite(Guid gameQueueId, dynamic formContent)
        {
            var users = formContent.users != null ?
                    ((JsonArray)formContent.users).ToObjectArray().Select(o => o.ToString()).ToList() :
                    null;
            string message = formContent.message != null ? formContent.message.Value : null;
            string url = formContent.url != null ? formContent.url.Value : null;

            this.gameRepository.Invite(this.CurrentUserId, gameQueueId, message, url, users);
        }

        private string GetCommandDataValue(IDictionary<string, object> commandData, string commandDataKey)
        {
            if (!commandData.ContainsKey(commandDataKey))
            {
                throw new InvalidOperationException(commandDataKey + " parameter cannot be null or empty");
            }

            return commandData[commandDataKey].ToString();
        }
    }
}
