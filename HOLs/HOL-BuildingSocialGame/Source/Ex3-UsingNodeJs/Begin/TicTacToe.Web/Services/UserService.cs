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
    using System.Json;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Mvc;
    using Microsoft.Samples.SocialGames.Entities;
    using Microsoft.Samples.SocialGames.Repositories;

    public class UserService : ServiceBase, IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IStatisticsRepository statsRepository;

        public UserService(IUserRepository userRepository, IStatisticsRepository statsRepository, IUserProvider userProvider)
            : base(userProvider)
        {
            this.userRepository = userRepository;
            this.statsRepository = statsRepository;
        }

        public string Verify()
        {
            if (string.IsNullOrWhiteSpace(this.CurrentUserId))
            {
                throw new ServiceException("The user is not authenticated");
            }

            return this.CurrentUserId;
        }

        public void UpdateProfile(dynamic formContent)
        {
            var displayName = string.Empty;
            
            if (formContent != null)
            {
                displayName = (string)(formContent.displayName ?? string.Empty);
            }

            var userProfile = this.userRepository.GetUser(CurrentUserId);

            if (userProfile == null)
            {
                throw new ServiceException("User does not exist");
            }

            if (!string.IsNullOrWhiteSpace(displayName))
            {
                userProfile.DisplayName = displayName;
            }

            this.userRepository.AddOrUpdateUser(userProfile);
        }

        public Board Leaderboard(int count)
        {
            throw new NotImplementedException();
        }

        public string[] GetFriends()
        {
            return this.userRepository.GetFriends(this.CurrentUserId).ToArray();
        }

        public UserInfo[] GetFriendsInfo()
        {
            return this.userRepository.GetFriendsInfo(this.CurrentUserId).ToArray();
        }

        public void AddFriend(string friendId)
        {
            this.userRepository.AddFriend(this.CurrentUserId, friendId);
        }

        private void UpdateUserName(ref Board board)
        {
            foreach (var score in board.Scores)
            {
                if (string.IsNullOrEmpty(score.UserId))
                {
                    continue;
                }

                var profile = this.userRepository.GetUser(score.UserId);

                if (profile != null && !string.IsNullOrEmpty(profile.DisplayName))
                {
                    score.UserName = profile.DisplayName;
                }
                else 
                {
                    score.UserName = score.UserId;
                }
            }
        }
    }
}
