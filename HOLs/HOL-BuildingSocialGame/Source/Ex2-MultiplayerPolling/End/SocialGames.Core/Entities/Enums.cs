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

namespace Microsoft.Samples.SocialGames.Entities
{
    public enum GameStatus
    {
        Waiting,        // Still waiting for all participants 
        Ready,          // Your game is ready
        Timeout,        // Not enough available people to play
        GameStarted,    // The game you want has started without you
        GameOver,       // Your game is already over
        NotFound        // Incorrect ID or game is not valid
    }

    public enum QueueStatus
    {
        Waiting,        // Still waiting for all participants
        NotFound,       // Queue id is invalid
        Ready,          // Your game is ready, user will be put in the war room for weapon selection
        Timeout         // Not enough available people to play
    }

    public enum GameType
    {
        Skirmish,
        Ranking,
        Invitational
    }
}