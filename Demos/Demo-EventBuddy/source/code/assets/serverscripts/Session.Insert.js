function insert(item, user, request) { 
      item.userId = user.userId; 
 
      if (item.speaker) { 
          // get these from https://dev.twitter.com/apps
            var twitterConsumerKey = 'YourAppsConsumerKey';
            var twitterConsumerSecret = 'YourAppsConsumerSecret';
             
            // This works for users signed in with Twitter auth, otherwise
            // you can use your own values for this from https://dev.twitter.com/apps 
            // these are on the same page under "your access token" section
            var identities = user.getIdentities();
            var userToken = identities.twitter.accessToken;
            var userSecret = identities.twitter.accessTokenSecret;
             
            var OAuth = require('OAuth');
            var oauth = new OAuth.OAuth(
                  'https://api.twitter.com/oauth/request_token',
                  'https://api.twitter.com/oauth/access_token',
                  twitterConsumerKey,
                  twitterConsumerSecret,
                 '1.0A',
                  null,
                  'HMAC-SHA1'
                );
            
            oauth.get("https://api.twitter.com/1.1/users/show.json?screen_name=" + item.speaker, userToken, userSecret, 
            function(error, data) {
                //console.log("error: ", error);
                console.log("data: ", data);
                
                if (data) {                    
                    var json = JSON.parse(data);
                    if (json.profile_image_url) {
                        var biggerImg = json.profile_image_url.replace("normal", "bigger");
                        item.img = biggerImg;
                        request.execute();
                    }
                } else {
                    item.img = "Assets/NoProfile.png"; 
                    request.execute(); 
                }
            });                                
      } 
      else { 
             item.img = "Assets/NoProfile.png"; 
             request.execute(); 
      } 
} 